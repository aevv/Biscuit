namespace BiscuitHeaders
{
    public enum ServerHeaders : short
    {
        // Login
        Test = -1,
        Login = 0,
        Logout,

        // Chat
        ChatMessage,

        // World
        MoveCharacter,
        RequestMap,

        // Character Selection
        RequestCharacters,
        SelectCharacter,
        CreateCharacter,
        DeleteCharacter
    }
}
