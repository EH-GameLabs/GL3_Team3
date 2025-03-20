
#region TAGS
public class Tags
{
    public static readonly string Player = "Player";
    public static readonly string Enemy = "Enemy";
}
#endregion

#region ENUMS

#region GUNS
public enum GunType
{
    Primary,
    Secondary,
}
#endregion

#region ENEMY
public enum EnemyBehaviour
{
    Area,
    All
}

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
}
#endregion

#region ITEMS
public enum ItemsType
{
    Weapon,
    Ammo,
    Key,
    Hostage
}
#endregion

#region DOORS
public enum DoorsType
{
    Closed,
    Secret,
    Key
}
#endregion
#endregion