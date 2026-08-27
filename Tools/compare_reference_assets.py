"""Compare assets recovered from a Unity build with an extracted unitypackage.

The script is intentionally read-only.  A unitypackage extraction stores each
asset below a GUID directory as ``asset`` plus a human-readable ``pathname``.
AssetRipper exports the build as ordinary files, so matching uses file names and,
for PNG files, decoded RGBA pixels rather than PNG container bytes.
"""

from __future__ import annotations

import argparse
import hashlib
import json
from pathlib import Path

import numpy as np
from PIL import Image


def sha256(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest()


def png_fingerprint(path: Path) -> tuple[tuple[int, int], str, np.ndarray]:
    with Image.open(path) as image:
        rgba = image.convert("RGBA")
        pixels = np.asarray(rgba, dtype=np.int16)
        return rgba.size, sha256(rgba.tobytes()), pixels


def visible_pixel_similarity(left: np.ndarray, right: np.ndarray) -> float:
    """Return 0..1 similarity while ignoring RGB hidden by full transparency."""
    visible = (left[:, :, 3] > 0) | (right[:, :, 3] > 0)
    if not visible.any():
        return 1.0
    error = np.abs(left[visible] - right[visible]).mean()
    return float(1.0 - error / 255.0)


def reference_files(root: Path):
    for directory in root.iterdir():
        pathname = directory / "pathname"
        asset = directory / "asset"
        if pathname.is_file() and asset.is_file():
            yield pathname.read_text(encoding="utf-8").strip(), asset


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("reference", type=Path)
    parser.add_argument("build_assets", type=Path)
    args = parser.parse_args()

    build_by_name: dict[str, list[Path]] = {}
    for path in args.build_assets.rglob("*"):
        if path.is_file() and not path.name.endswith(".meta"):
            build_by_name.setdefault(path.name.casefold(), []).append(path)

    records = []
    for source_path, reference in reference_files(args.reference):
        suffix = Path(source_path).suffix.casefold()
        if suffix not in {".png", ".anim", ".controller", ".cs"}:
            continue

        candidates = build_by_name.get(Path(source_path).name.casefold(), [])
        record = {
            "reference": source_path,
            "type": suffix,
            "candidates": [],
        }
        for candidate in candidates:
            candidate_record = {
                "build": candidate.relative_to(args.build_assets).as_posix(),
                "byte_equal": sha256(reference.read_bytes())
                == sha256(candidate.read_bytes()),
            }
            if suffix == ".png":
                ref_size, ref_hash, ref_pixels = png_fingerprint(reference)
                build_size, build_hash, build_pixels = png_fingerprint(candidate)
                same_size = ref_size == build_size
                candidate_record.update(
                    {
                        "reference_size": list(ref_size),
                        "build_size": list(build_size),
                        "pixel_equal": same_size and ref_hash == build_hash,
                        "visible_pixel_similarity": round(
                            visible_pixel_similarity(ref_pixels, build_pixels), 6
                        )
                        if same_size
                        else None,
                    }
                )
            record["candidates"].append(candidate_record)
        records.append(record)

    summary = {
        "reference_candidates": len(records),
        "same_name_matches": sum(bool(r["candidates"]) for r in records),
        "png_reference_count": sum(r["type"] == ".png" for r in records),
        "png_pixel_matches": sum(
            any(c.get("pixel_equal") for c in r["candidates"])
            for r in records
            if r["type"] == ".png"
        ),
        "png_probable_matches_98pct": sum(
            any((c.get("visible_pixel_similarity") or 0) >= 0.98 for c in r["candidates"])
            for r in records
            if r["type"] == ".png"
        ),
    }
    print(json.dumps({"summary": summary, "records": records}, ensure_ascii=False, indent=2))


if __name__ == "__main__":
    main()
