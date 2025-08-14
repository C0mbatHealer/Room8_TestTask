
namespace TestSuitBaseShooterMechanics
{
    public static class HelpersExt
    {
        /// <summary>
        /// Wrapper for Game Driver ApiClient.StartEditorPlay
        /// </summary>
        /// <param name="timeout">Timeout in sec for call method StartEditorPlay. Default value is 30</param>
        public static void StartEditorPlayExt(this ApiClient api, int timeout = 30)
        {
            Assert.IsTrue(api.StartEditorPlay(timeout), "TEST.ERROR: Can't start editor play.");
        }

        /// <summary>
        /// Wrapper for Game Driver ApiClient.StopEditorPlay
        /// </summary>
        /// <param name="timeout">Timeout in sec for call method StopEditorPlay. Default value is 30 </param>
        public static void StopEditorPlayExt(this ApiClient api, int timeout = 30)
        {
            Assert.IsTrue(api.StopEditorPlay(timeout), "TEST.ERROR: Can't stop editor play.");
        }

        /// <summary>
        /// Wrapper for ApiClient.LoadLevel with Checking result
        /// </summary>
        /// <param name="levelPath">String path to loading level</param>
        public static void LoadLevelExt(this ApiClient api, string levelPath)
        {
            Assert.IsTrue(api.LoadLevel(levelPath), $"TEST.ERROR: Level: {levelPath} not loaded.");
            api.Wait(10000); // Timeout for loading level
        }

        /// <summary>
        /// Check for object exist.
        /// </summary>
        /// <param name="api">Ref to ApiClient</param>
        /// <param name="objectNameId">String of object name</param>
        /// <param name="timeout">Timeout in sec for waiting of object checking</param>
        public static void CheckObjectExist(this ApiClient api, string objectNameId, int timeout = 30)
        {
            Assert.IsTrue(api.WaitForObject($"//{objectNameId}", timeout), $"TEST.FAIL: Object [{objectNameId}] not exist.");
        }

        /// <summary>
        /// Teleport character to new position
        /// </summary>
        /// <param name="api">Ref to ApiClient</param>
        /// <param name="characterNameId">String of actor name</param>
        /// <param name="position">New position for teleport</param>
        public static void CharacterTeleportTo(this ApiClient api, string characterNameId, Vector3 position)
        {
            api.CheckObjectExist(characterNameId);

            Vector3 rotation = api.CallMethod<Vector3>($"//{characterNameId}", "K2_GetActorRotation", new object[] { });
            api.CallMethod($"//{characterNameId}", "K2_TeleportTo", new object[] { position, rotation });
        }

        /// <summary>
        /// Print text on game screen and ingame console and Visual Studio output console
        /// </summary>
        /// <param name="api">Ref to ApiClient</param>
        /// <param name="text">Text to print</param>
        /// <param name="duration">Time to show text on screen in sec</param>
        public static void PrintText(this ApiClient api, string text, float duration)
        {
            api.ConsoleCommand($"Test_DrawText \"{text}\" {duration}");
            Console.WriteLine($"TEST.INFO: {text}");
        }
    }
}
