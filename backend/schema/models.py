from enum import Enum
from typing import Literal, TypeAlias


class YoloModelName(Enum):
    YOLOV8_NANO = "yolov8n.pt"
    YOLOV8_SMALL = "yolov8s.pt"
    YOLOV8_MEDIUM = "yolov8m.pt"
    YOLOV8_LARGE = "yolov8l.pt"
    YOLOV8X = "yolov8x.pt"

    


AllModelEnum: TypeAlias = (
    "YoloModelName"
)