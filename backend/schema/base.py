from pydantic import BaseModel, Field
from typing import Any, Dict, List, Optional, Union

class UserInput(BaseModel):
    image: str
    model_name: str

