
import { Injectable } from '@angular/core';
import { HammerGestureConfig } from '@angular/platform-browser';
import { DIRECTION_HORIZONTAL, Pinch, Rotate, Swipe } from 'hammerjs';

@Injectable()
export class CustomHammerConfig extends HammerGestureConfig {
  override  overrides = {
    pan: {
        direction: DIRECTION_HORIZONTAL 
    },
    pinch: {
        enable: false
    },
    rotate: {
        enable: false
    }
};
}