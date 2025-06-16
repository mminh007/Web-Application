from fastapi import FastAPI, Request
from fastapi.middleware.cors import CORSMiddleware
from logs.logger_factory import get_logger
from backend.api.routes.yolo_predict import route as detect_router

logger = get_logger("backend", "backend.log")

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],  # Cẩn thận khi đưa vào production
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

app.include_router(detect_router, tag=["object-detection"])