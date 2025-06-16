from schema.models import YoloModelName, AllModelEnum
from functools import cache 
from ultralytics import YOLO
from logs.logger_factory import get_logger

logger = get_logger("llm", "llm.log")


_MODEL_map = dict[AllModelEnum, type] = {
    YoloModelName.YOLOV8X: "yolov8x.pt",
    YoloModelName.YOLOV8_LARGE: "yolov8l.pt",
    YoloModelName.YOLOV8_MEDIUM: "yolov8m.pt",
    YoloModelName.YOLOV8_SMALL: "yolov8s.pt",
    YoloModelName.YOLOV8_NANO: "yolov8n.pt",

    
}

@cache
def get_model(model_name: AllModelEnum):
    
    try:
        if isinstance(model_name, str):
            for enum in AllModelEnum:
                try:
                    model_name = enum(model_name)
                except ValueError:
                    continue
        else:
            logger.error(f"Invalid model name: {model_name}. Expected a string.")
            return None
        
        api_model_name = _MODEL_map[model_name]
        logger.info(f"Model name for {model_name} is {api_model_name}.")

        if model_name in YoloModelName:
            return YOLO(api_model_name)


    except KeyError:
        logger.error(f"Model {model_name} not found in the model map.")
        return None
    