public static class StaticInput
{
    private static StandartInputActions _singletone;
    public static StandartInputActions Singletone
    {
        get
        {
            if (_singletone == null)
            {
                _singletone = new();
                _singletone.Standart.Enable();
            }
            return _singletone;
        }
    }
}
