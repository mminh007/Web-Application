from core.llm import get_model
from schema.base import UserInput
import base64
from PIL import Image
import numpy as np
from logs.logger_factory import get_logger
from fastapi import HTTPException
import io
import traceback

logger = get_logger("yolo", "yolo.log")

async def yolo_predict(user_input: UserInput):
    model_name = user_input.model_name
    image = user_input.image

    try:
        header, image_decode = image.split(",", 1)
        image = base64.b64decode(image_decode)
        image = Image.open(io.BytesIO(image)).convert("RGB")
        image = np.array(image)

        model = get_model(model_name)

        if model is None:
            logger.error(f"❌ get_model() returned None for model: {model_name}")
            raise ValueError(f"Invalid or unsupported model: {model_name}")
        
        response = model(image)

        return response

    except Exception as e:
        tb = traceback.format_exc()
        logger.error(f"❌ Error in yolo model: {str(e)}\n{tb}")
        raise HTTPException(status_code=400, detail=f"Invalid image data: {str(e)}")
    
