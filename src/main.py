from __future__ import annotations

from fastapi import FastAPI

from src.app.presentation.api.routers.rules import router as rules_router

app = FastAPI(title="LoreWeaver API", version="0.1.0")
app.include_router(rules_router)
