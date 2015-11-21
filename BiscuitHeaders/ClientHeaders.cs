namespace BiscuitHeaders
{
    public enum ClientHeaders : short
    {

        // Login.
        RequestLogin = 0,
        LoginResult,

        // Chat.
        ChatMessage,

        // Char Select.
        SelectableCharacter,
        CharacterSelectionResult,
        CharacterCreationResult,
        CharacterDeletionResult,

        // World.
        SetMap,
        GiveChunk,
        FinishMap,

        // Utility.
        ServerOffline,

        // Entities
        EntityLocation,
        RemoveEntity,
    }
}
