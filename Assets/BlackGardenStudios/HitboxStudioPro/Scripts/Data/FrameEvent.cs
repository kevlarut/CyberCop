using System;

namespace BlackGardenStudios.HitboxStudioPro
{
    [Serializable]
    public enum FrameEvent
    {
        PROJECTILE_1 = 0, 
        PROJECTILE_2 = 1, 
        PROJECTILE_3 = 2, 
        PROJECTILE_4 = 3, 
        JUMP = 4, 
        ENABLE_COLLISIONS = 5, 
        DISABLE_COLLISIONS = 6, 
        CLEAR_INPUTS = 7, 
        RESET_TARGETS = 8, 
        ENABLE_MOVE = 9, 
        DISABLE_MOVE = 10, 
        ENABLE_DIRECTION = 11, 
        DISABLE_DIRECTION = 12, 
        SET_BOUNCE = 13, 
        SET_HEIGHT = 14, 
        PLAY_SOUND = 15, 
        SET_POISE_DAMAGE = 16, 
        SET_ATTACK_DAMAGE = 17
    }
}