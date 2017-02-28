using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public enum csLifeStagesEnum
{
    Baby = 1,
    Teen = 2,
    Adult = 3
}

public enum csMessageStatusEnum
{
    None = 0,
    Visible = 1,
    Hidden = 2,
    InProgress = 3
}

public enum csMessageTypeEnum
{
    Success = 0,
    Failure = 1,
    Info = 2,
    Warning = 3
}

public enum csBaseStatus
{
    Strength = 0,
    Dexterity = 1,
    Inteligence = 2
}

public enum csEffect
{
    Freeze = 0,
    Damage = 1,
    AffectStatus = 2,
    Heal = 3,
    Cure = 4,
    ExtraDamage = 5
}

public enum csCorePower
{
    Energy = 0,
    Fire = 1,
    Ice = 2,
    Water = 3,
    Wind = 4
}

public enum csAttack
{
    HighKick = 0,
    Punch = 1,
    LowKick = 2
}

public enum csAttackType
{
    Power = 0,
    MartialArts = 1
}