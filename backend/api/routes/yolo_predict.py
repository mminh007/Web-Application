from fastapi import FastAPI, APIRouter, Request, HTTPException
from logs.logger_factory import get_logger
from contextlib import asynccontextmanager
from schema.base import UserInput
from detect_model.yolo import yolo_predict
from controller.utils import process_output

logger = get_logger("yolo-detect", "yolo-detect.log")

@asynccontextmanager
async def lifespan(app: FastAPI) -> AssertionError:
    pass

router = APIRouter(lifespan=lifespan)


@router.post("/detect")
def predict(request: Request, user_input: UserInput):

    result = yolo_predict(user_input)

    image = user_input.image

    
    
    

    

    