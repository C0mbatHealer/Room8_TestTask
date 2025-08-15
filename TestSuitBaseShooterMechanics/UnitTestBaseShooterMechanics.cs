
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

using NUnit.Framework.Internal;

namespace TestSuitBaseShooterMechanics
{
    public class Tests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            InitApi();
            api.StartEditorPlayExt();
            api.LoadLevelExt("/ShooterMaps/Maps/L_FiringRange_WP");
            api.CreateInputDevice("GDIO", "IMC_Default");
        }

        [SetUp]
        public void TestCaseSetup()
        {
            // Default pause between tests
            api.Wait(2000);
        }

        [OneTimeTearDown]
        public void Disconnect()
        {
            ReleaseApi();
        }

        /// <summary>
        /// Test Case ID: TC01 - Player Move
        /// Verify that the player character can move in all directions
        /// </summary>
        [Test, Order(0)]
        public void TestCharacterMove()
        {
            api.PrintText("TC01 - Player Move", 3);
            
            // Teleprot Player to test location 
            TeleportToMovementTestLocation();

            var playerName = "//B_Hero_ShooterMannequin_C_0";

            #region TC01 - Player Move: 1. Check player character is standing
            
            api.PrintText("TC01 - Player Move: 1. Check player character is standing", 3);

            // Gets player velocity
            Vector3 playerVelocity = api.CallMethod<Vector3>(playerName, "Test_GetVelocity");
            
            // Validate player velocity is 0
            Assert.That(playerVelocity.x == 0, 
                        Is.True, $"TEST.FAILT: Expected velocity.x == 0, but velocity.x was {playerVelocity.x}");
            Assert.Multiple(() =>
            {
                Assert.That(playerVelocity.y == 0,
                            Is.True, $"TEST.FAILT: Expected velocity.y == 0, but velocity.x was {playerVelocity.y}");
                Assert.That(playerVelocity.z == 0,
                            Is.True, $"TEST.FAILT: Expected velocity.z == 0, but velocity.x was {playerVelocity.z}");
            });

            #endregion

            #region TC01 - Player Move: 2. Check player can move forward
            
            api.PrintText("TC01 - Player Move: 2. Check player can move forward", 3);
            
            // Try move forward
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.W }, 0));
            api.Wait(500);

            // Gets player velocity
            playerVelocity = api.CallMethod<Vector3>( playerName, "Test_GetVelocity");

            // Validate move forward
            Assert.That(playerVelocity.x < 0,
                        Is.True, $"TEST.FAILT: Expected velocity.x < 0, but velocity.x was {playerVelocity.x}");

            api.Wait(500);

            // Try stop move
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.W }, 0));

            #endregion

            #region TC01 - Player Move: 3. Check player can move backward
            
            api.PrintText("TC01 - Player Move: 3. Check player can move backward", 3);

            // Try move backward
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.S }, 0));
            api.Wait(500);
            
            // Get player velocity
            playerVelocity = api.CallMethod<Vector3>(playerName, "Test_GetVelocity");
            
            // Validate move backward
            Assert.That(playerVelocity.x > 0,
                        Is.True, $"TEST.FAILT: Expected velocity.x > 0, but velocity.x was {playerVelocity.x}");

            api.Wait(500);

            // Try stop move
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.S }, 0));

            #endregion

            #region TC01 - Player Move: 4. Check player can strafe right
            
            api.PrintText("TC01 - Player Move: 4. Check player can strafe right", 3);

            // Try strafe right
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.D }, 0));
            api.Wait(500);

            // Gets player velocity
            playerVelocity = api.CallMethod<Vector3>(playerName, "Test_GetVelocity");
            
            // Validate strafe right
            Assert.That(playerVelocity.y < 0,
                        Is.True, $"TEST.FAILT: Expected velocity.y < 0, but velocity.x was {playerVelocity.y}");

            api.Wait(500);

            // Try stop move
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.D }, 0));

            #endregion

            #region TC01 - Player Move: 5. Check player can strafe left
            
            api.PrintText("TC01 - Player Move: 5. Check player can strafe left", 3);

            // Try strafe left
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.A }, 0));
            api.Wait(500);

            // Gets player velocity
            playerVelocity = api.CallMethod<Vector3>(playerName, "Test_GetVelocity");
            
            // Validate strafe left
            Assert.That(playerVelocity.y > 0,
                        Is.True, $"TEST.FAILT: Expected velocity.y > 0, but velocity.x was {playerVelocity.y}");

            api.Wait(500);

            // Try stop move
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.A }, 0));

            #endregion

            Assert.Pass();
        }

        /// <summary>
        /// Test Case ID: TC02 - Player Jump
        /// Check if the player can jump
        /// </summary>
        [Test, Order(1)]
        public void TestCharacterJump()
        {
            api.PrintText("TC02 - Player Jump", 3);

            // Teleprot Player to test location 
            TeleportToMovementTestLocation();

            var playerName = "//B_Hero_ShooterMannequin_C_0";

            #region TC02 - Player Jump: 1. Check player can jump
            
            api.PrintText("TC02 - Player Jump: 1. Check player can jump", 3);

            // Validate player character CanJump property
            bool canJump = api.CallMethod<bool>(playerName, "CanJump");
            Assert.That(canJump, Is.True, 
                $"TEST.FAIL: Expected canJump is TRUE, but canJump was {canJump}");

            api.Wait(1000);

            // Gets initial jump count (should be 0)
            int jumpCountStart = api.CallMethod<int>(playerName, "Test_GetJumpingCurrentCount");
            
            // Validate player is not in jump state
            Assert.That(jumpCountStart == 0, 
                Is.True, $"TEST.FAIL: Expected jumpCountStart == 0, but jumpCountStart was {jumpCountStart}");

            // Try jump
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.SpaceBar }, 0));
            api.Wait(300);

            // Gets player jumped count property
            int jumpCountCurrent = api.CallMethod<int>(playerName, "Test_GetJumpingCurrentCount");
            
            api.Wait(300);

            // Try release jump action key
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.SpaceBar }, 0));

            // Validate player jumped
            Assert.That(jumpCountCurrent > jumpCountStart, 
                Is.True, $"TEST.FAIL: Expected jumpCountCurrent > 0, but jumpCountCurrent was {jumpCountCurrent}");

            #endregion

            #region TC02 - Player Jump: 2. Check player character movement doesn't interrupt jump
            
            api.PrintText("TC02 - Player Jump: 2. Check player character movement doesn't interrupt jump", 3);
            
            // Start moving forward
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.W }, 0));
            api.Wait(500);
            
            // Gets player velocity
            Vector3 velocity = api.CallMethod<Vector3>(playerName, "Test_GetVelocity");
            
            // Validate player moving
            Assert.That(velocity.x < 0,
                        Is.True, $"TEST.FAILT: Expected velocity.x < 0, but velocity.x was {velocity.x}");

            // Try jump
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.W, KeyCode.SpaceBar }, 0));
            api.Wait(300);

            // Gets player jumped count property
            jumpCountCurrent = api.CallMethod<int>(playerName, "Test_GetJumpingCurrentCount");
            
            api.Wait(300);

            // Try release jump action key
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.SpaceBar }, 0));

            // Validate player jumped
            Assert.That(jumpCountCurrent > jumpCountStart,
                Is.True, $"TEST.FAIL: Expected jumpCountCurrent > 0, but jumpCountCurrent was {jumpCountCurrent}");

            #endregion

            Assert.Pass();
        }

        /// <summary>
        /// Test Case ID: TC03 - Player Crouch
        /// Assess if the player can crouch
        /// </summary>
        [Test, Order(2)]
        public void TestCharacterCrouch()
        {
            api.PrintText("TC03 - Player Crouch", 3);

            // Teleprot Player to test location 
            TeleportToMovementTestLocation();

            var playerName = "B_Hero_ShooterMannequin_C_0";

            #region TC03 - Player Crouch: 1. Check player character crouch
            
            api.PrintText("TC03 - Player Crouch: 1. Check player character crouch", 3);

            // Validate player character CanCrouch property
            bool canCrouch = api.CallMethod<bool>($"//{playerName}", "CanCrouch");
            Assert.That(canCrouch,
                Is.True, $"TEST.FAIL: Expected canCrouch is TRUE, but canCrouch was {canCrouch}");

            api.Wait(1000);

            // Try crouch
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.LeftControl }, 0));
            api.Wait(300);
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.LeftControl }, 0));
            
            // Validate player in crouch state
            bool isCrouched = api.GetObjectFieldValue<bool>($"//{playerName}/@bIsCrouched");
            Assert.That(isCrouched,
                Is.True, $"TEST.FAIL: Expected IsCrouched is TRUE, but IsCrouched was {isCrouched}");
            
            api.Wait(1000);

            #endregion

            #region TC03 - Player Crouch: 2. Check player character moving doesn't interrupt crouch state
            
            api.PrintText("TC03 - Player Crouch: 2. Check player character moving doesn't interrupt crouch state", 3);
            
            // Strafe left
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.A }, 0));
            api.Wait(1000);
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.A }, 0));

            // Validate player in crouch state
            isCrouched = api.GetObjectFieldValue<bool>($"//{playerName}/@bIsCrouched");
            Assert.That(isCrouched,
                Is.True, $"TEST.FAIL: Expected IsCrouched is TRUE, but IsCrouched was {isCrouched}");
            
            // Strafe right
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.D }, 0));
            api.Wait(1000);
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.D }, 0));

            // Validate player in crouch state
            isCrouched = api.GetObjectFieldValue<bool>($"//{playerName}/@bIsCrouched");
            Assert.That(isCrouched,
                Is.True, $"TEST.FAIL: Expected IsCrouched is TRUE, but IsCrouched was {isCrouched}");

            // Move backward
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.S }, 0));
            api.Wait(1000);
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.S }, 0));

            // Validate player in crouch state
            isCrouched = api.GetObjectFieldValue<bool>($"//{playerName}/@bIsCrouched");
            Assert.That(isCrouched,
                Is.True, $"TEST.FAIL: Expected IsCrouched is TRUE, but IsCrouched was {isCrouched}");
            
            // Move forward
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.W }, 0));
            api.Wait(1000);
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.W }, 0));

            // Validate player in crouch state
            isCrouched = api.GetObjectFieldValue<bool>($"//{playerName}/@bIsCrouched");
            Assert.That(isCrouched,
                Is.True, $"TEST.FAIL: Expected IsCrouched is TRUE, but IsCrouched was {isCrouched}");

            #endregion

            #region TC03 - Player Crouch: 3. Check player character uncrouch
            
            api.PrintText("TC03 - Player Crouch: 3. Check player character uncrouch", 3);

            // Try uncrouch
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.LeftControl }, 0));
            api.Wait(300);
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.LeftControl }, 0));

            // Validate player not in crouch state
            isCrouched = api.GetObjectFieldValue<bool>($"//{playerName}/@bIsCrouched");
            Assert.That(isCrouched,
                Is.False, $"TEST.FAIL: Expected IsCrouched is FALSE, but IsCrouched was {isCrouched}");
            
            api.Wait(1000);

            #endregion

            #region TC03 - Player Crouch: 4. Check player character uncrouch by jump
            
            api.PrintText("TC03 - Player Crouch: 4. Check player character uncrouch by jump", 3);
            
            // Try crouch
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.LeftControl }, 0));
            api.Wait(300);
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.LeftControl }, 0));

            api.Wait(1000);

            // Validate player in crouch state
            isCrouched = api.GetObjectFieldValue<bool>($"//{playerName}/@bIsCrouched");
            Assert.That(isCrouched,
                Is.True, $"TEST.FAIL: Expected IsCrouched is TRUE, but IsCrouched was {isCrouched}");
            
            // Try jump
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.SpaceBar }, 0));
            api.Wait(300);
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.SpaceBar }, 0));

            // Validate player not in crouch state
            isCrouched = api.GetObjectFieldValue<bool>($"//{playerName}/@bIsCrouched");
            Assert.That(isCrouched,
                Is.False, $"TEST.FAIL: Expected IsCrouched is FALSE, but IsCrouched was {isCrouched}");

            #endregion

            Assert.Pass();
        }

        /// <summary>
        /// Test Case ID: TC04 - Player can shoot
        /// Test if the player can fire their weapon and hit a target
        /// </summary>
        [Test, Order(3)]
        public void TestGunfire()
        {
            api.PrintText("TC04 - Player can shoot", 3);

            // Teleprot Player to test location 
            TeleportToShootingTestLocation();

            //Set default player character name
            var playerName = "B_Hero_ShooterMannequin_C_0";

            // Find player character TeamID
            int playerTeamId = api.CallMethod<int>(
                $"//{playerName}", 
                "Test_GetCharacterTeamID", 
                new object[] { playerName }
                );
            
            api.Wait(1000);

            // Find enemy from other characters
            string[] characters = new string[] 
            {
                "B_Hero_ShooterMannequin_C_1",
                "B_Hero_ShooterMannequin_C_2",
                "B_Hero_ShooterMannequin_C_3"
            };
            string enemyCharacter = GetFirstEnemyCharacterName(characters, playerTeamId);

            // Get enemy character health
            double enemyHealth = api.CallMethod<double>($"//{enemyCharacter}", "Test_GetCharacterHealth");
            
            api.Wait(1000);

            // Get player weapon ammo count
            int playerAmmoCount = api.CallMethod<int>($"//{playerName}", "Test_GetCharacterWeaponAmmoCount");

            api.Wait(1000);

            // Set player character look at enemy
            api.CallMethod($"//{playerName}", "Test_CharacterLookAt", new object[] { enemyCharacter });
            api.Wait(1000);
            api.CallMethod($"//{playerName}", "Test_DisableLookAt");

            // Player shoot at enemy character
            api.CallMethod($"//{playerName}", "Test_UseAbilityFireWeapon");

            api.Wait(1000);

            #region TC04 - Player can shoot: 1. Validate player weapon ammo count decreased

            api.PrintText("TC04 - Player can shoot: 1. Validate player weapon ammo count decreased", 3);

            // Gets current ammo count
            int validateAmmoCount = api.CallMethod<int>($"//{playerName}", "Test_GetCharacterWeaponAmmoCount");

            api.Wait(1000);

            // Validate ammo count is decreased
            Assert.That(validateAmmoCount < playerAmmoCount,
                Is.True, $"TEST.FAIL: Expected validateAmmoCount < playerAmmoCount, but validateAmmoCount was {validateAmmoCount}; " +
                         $"playerAmmoCount was {playerAmmoCount}");

            #endregion

            #region TC04 - Player can shoot: 2. Validate enemy is hit

            api.PrintText("TC04 - Player can shoot: 2. Validate enemy is hit", 3);

            // Gets enemy character health count
            double validateEnemyHealth = api.CallMethod<double>($"//{enemyCharacter}", "Test_GetCharacterHealth");

            api.Wait(1000);

            // Validate enemy character health decreased
            Assert.That(validateEnemyHealth < enemyHealth, 
                Is.True, $"TEST.FAIL: Expected validateEnemyHealth < enemyHealth, but validateEnemyHealth was {validateEnemyHealth}; " +
                         $"enemyHealth was {enemyHealth}");

            #endregion

            Assert.Pass();
        }

        /// <summary>
        /// Test Case ID: TC05 - Player Get Damage
        /// Confirm that the player receives damage
        /// </summary>
        [Test, Order(4)]
        public void TestCharacterRecieveDamage()
        {
            api.PrintText("TC05 - Player Get Damage", 3);

            //Set default player character name
            var playerName = "B_Hero_ShooterMannequin_C_0";

            #region TC05 - Player Get Damage: 1. From damage zone

            api.PrintText("TC05 - Player Get Damage: 1. From damage zone", 3);

            // Gets player current health count
            double playerHealth = api.CallMethod<double>($"//{playerName}","Test_GetCharacterHealth");

            api.Wait(1000);

            // Teleport to damage zone
            api.CharacterTeleportTo(playerName, new Vector3(204, -2774, 807));
            api.Wait(2000);

            // Teleport to safe zone
            api.CharacterTeleportTo(playerName, new Vector3(-935, -2133, 797));
            api.Wait(1000);

            // Gets player health count
            double validatePlayerHealth = api.CallMethod<double>($"//{playerName}", "Test_GetCharacterHealth");

            api.Wait(1000);

            // Validate player health count
            Assert.That(validatePlayerHealth < playerHealth, 
                Is.True, $"TEST.FAIL: Expected validatePlayerHealth < playerHealth, but validatePlayerHealth was {validatePlayerHealth}; " +
                         $"playerHealth was {playerHealth}");

            #endregion

            #region C05 - Player Get Damage: 1.1 Restore player health

            api.PrintText("TC05 - Player Get Damage: 1.1 Restore player health", 3);

            // Teleport player to healing zone
            api.CharacterTeleportTo(playerName, new Vector3(148, -2325, 807));
            api.Wait(1000);

            // Get player max health count
            double playerMaxHealth = api.CallMethod<double>($"//{playerName}", "Test_GetCharacterMaxHealth");

            api.Wait(1000);

            // Get player health
            validatePlayerHealth = api.CallMethod<double>($"//{playerName}", "Test_GetCharacterHealth");

            api.Wait(1000);

            // Validate player is restored own health
            Assert.That(validatePlayerHealth == playerMaxHealth,
                Is.True, $"TEST.FAIL: Expected validatePlayerHealth == playerMaxHealth, by validatePlayerHealth was {validatePlayerHealth}; " +
                         $"playerMaxHealth was {playerMaxHealth}");

            api.Wait(1000);

            #endregion

            #region TC05 - Player Get Damage: 2. From gunshots

            api.PrintText("TC05 - Player Get Damage: 2. From gunshots", 3);

            // Find player character TeamID
            int playerTeamId = api.CallMethod<int>(
                $"//{playerName}",
                "Test_GetCharacterTeamID",
                new object[] { playerName }
                );

            api.Wait(1000);

            // Find enemy from other characters
            string[] characters = new string[]
            {
                "B_Hero_ShooterMannequin_C_1",
                "B_Hero_ShooterMannequin_C_2",
                "B_Hero_ShooterMannequin_C_3"
            };
            string enemyCharacter = GetFirstEnemyCharacterName(characters, playerTeamId);

            // Teleport player to test position
            api.CharacterTeleportTo(playerName, new Vector3(-222, 1418, 93));
            api.Wait(1000);

            // Teleport enemy character to test position
            api.CharacterTeleportTo(enemyCharacter, new Vector3(-222, 862, 93));
            api.Wait(1000);

            // Set Player look at enemy
            api.CallMethod($"//{playerName}", "Test_CharacterLookAt", new object[] { enemyCharacter});
            api.Wait(1000);
            api.CallMethod($"//{playerName}", "Test_DisableLookAt");
            api.Wait(1000);

            // Set Enemy look at player
            api.CallMethod($"//{enemyCharacter}", "Test_CharacterLookAt", new object[] { playerName });
            api.Wait(2000);

            // Gets player current health count
            playerHealth = api.CallMethod<double>($"//{playerName}", "Test_GetCharacterHealth");

            // Enemy shoot to player
            api.CallMethod($"//{enemyCharacter}", "Test_UseAbilityFireWeapon");

            api.CallMethod($"//{enemyCharacter}", "Test_DisableLookAt");
            api.Wait(1000);

            // api.Wait(1000);

            // Gets player health
            validatePlayerHealth = api.CallMethod<double>($"//{playerName}", "Test_GetCharacterHealth");

            // Validate player recieve damage
            Assert.That(validatePlayerHealth < playerHealth,
                Is.True, $"TEST.FAIL: Expected validatePlayerHealth < playerHealth, but validatePlayerHealth was {validatePlayerHealth}; " +
                         $"playerHealth was {playerHealth}");

            // Teleport enemy character from test zone
            api.CharacterTeleportTo(enemyCharacter, new Vector3(-935, -2133, 797));
            api.Wait(1000);

            #endregion

            #region C05 - Player Get Damage: 2.1 Restore player health

            api.PrintText("TC05 - Player Get Damage: 2.1 Restore player health", 3);

            // Wait until healening zone recharged
            api.Wait(10000);

            // Teleport player to healing zone
            api.CharacterTeleportTo(playerName, new Vector3(148, -2325, 807));
            api.Wait(1000);

            // Teleport to test position
            api.CharacterTeleportTo(playerName, new Vector3(-222, 1418, 93));
            api.Wait(1000);

            // Get player health
            validatePlayerHealth = api.CallMethod<double>($"//{playerName}", "Test_GetCharacterHealth");

            api.Wait(1000);

            // Validate player is restored own health
            Assert.That(validatePlayerHealth == playerMaxHealth,
                Is.True, $"TEST.FAIL: Expected validatePlayerHealth == playerMaxHealth, by validatePlayerHealth was {validatePlayerHealth}; " +
                         $"playerMaxHealth was {playerMaxHealth}");

            api.Wait(1000);

            #endregion

            #region TC05 - Player Get Damage: 3. From grenade

            api.PrintText("TC05 - Player Get Damage: 4. From grenade", 3);

            // Teleport player to test position
            api.CharacterTeleportTo(playerName, new Vector3(-222, 1418, 93));
            api.Wait(1000);

            // Set Player look at wall
            api.CallMethod($"//{playerName}", "Test_CharacterLookAt", new object[] { "StaticMeshActor_27" });
            api.Wait(1000);
            api.CallMethod($"//{playerName}", "Test_DisableLookAt");
            api.Wait(1000);

            // Gets player current health count
            playerHealth = api.CallMethod<double>($"//{playerName}", "Test_GetCharacterHealth");

            api.Wait(1000);

            // Try use grenade
            // api.CallMethod($"//{playerName}", "Test_UseAbilityGrenade");
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.Q}, 0));
            api.Wait(300);
            Assert.IsTrue(api.KeyPress(new KeyCode[] { KeyCode.Q }, 0));

            api.Wait(3000);

            // Gets player health
            validatePlayerHealth = api.CallMethod<double>($"//{playerName}", "Test_GetCharacterHealth");

            // Validate player recieve damage
            Assert.That(validatePlayerHealth < playerHealth,
                Is.True, $"TEST.FAIL: Expected validatePlayerHealth < playerHealth, but validatePlayerHealth was {validatePlayerHealth}; " +
                         $"playerHealth was {playerHealth}");

            api.Wait(1000);

            #endregion

            #region TC05 - Player Get Damage: 4. From melee

            api.PrintText("TC05 - Player Get Damage: 3. From melee", 3);

            // Teleport player to test position
            api.CharacterTeleportTo(playerName, new Vector3(-222, 1418, 93));
            api.Wait(1000);

            // Teleport enemy character to test position
            api.CharacterTeleportTo(enemyCharacter, new Vector3(-230, 1342, 93));
            api.Wait(1000);

            // Set Player look at enemy
            api.CallMethod($"//{playerName}", "Test_CharacterLookAt", new object[] { enemyCharacter });
            api.Wait(1000);
            api.CallMethod($"//{playerName}", "Test_DisableLookAt");
            api.Wait(1000);

            // Set Enemy look at player
            api.CallMethod($"//{enemyCharacter}", "Test_CharacterLookAt", new object[] { playerName });
            api.Wait(1000);
            api.CallMethod($"//{enemyCharacter}", "Test_DisableLookAt");
            api.Wait(1000);

            // Gets player current health count
            playerHealth = api.CallMethod<double>($"//{playerName}", "Test_GetCharacterHealth");

            api.Wait(1000);

            // Enemy use melee atack on player
            api.CallMethod($"//{enemyCharacter}", "Test_UseAbilityMelee");

            // Gets player health
            validatePlayerHealth = api.CallMethod<double>($"//{playerName}", "Test_GetCharacterHealth");

            // Validate player recieve damage
            Assert.That(validatePlayerHealth < playerHealth,
                Is.True, $"TEST.FAIL: Expected validatePlayerHealth < playerHealth, but validatePlayerHealth was {validatePlayerHealth}; " +
                         $"playerHealth was {playerHealth}");

            api.Wait(1000);

            #endregion

            Assert.Pass();
        }

        #region Local Helpres

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(int hwnd);

        private static ApiClient api;

        /// <summary>
        /// Initialization for GameDriver API
        /// Check UE Editor is running and foregroud the UE Editor window for starting ATS
        /// </summary>
        public static void InitApi()
        {
            Process[] processes = Process.GetProcessesByName("UnrealEditor"); ;

            if (processes.Length > 0)
            {
                SetForegroundWindow((int)processes[0].MainWindowHandle);
            }
            else
            {
                Console.WriteLine($"TEST.ERROR. If you want to test in the editor, launch the editor and start the game first");
                Assert.Fail("TEST.ERROR: UnrealEditor not runned"); ;
            }

            api = new();
            api.Connect("localhost");
        }

        /// <summary>
        /// Disconnect AT from Unreal Engine Editor
        /// </summary>
        public static void ReleaseApi()
        {
            api.StopEditorPlayExt();
            api.Disconnect();
        }

        /// <summary>
        /// Teleport and rotation player character to position for movement tests
        /// </summary>
        public static void TeleportToMovementTestLocation()
        {
            api.CharacterTeleportTo("B_Hero_ShooterMannequin_C_0", new Vector3(284, 1069, 93));
            api.Wait(1000);
            api.CallMethod(
                "//B_Hero_ShooterMannequin_C_0", 
                "Test_CharacterLookAt", 
                new object[] { "StaticMeshActor_270" }
                );
            api.Wait(1000);
            api.CallMethod("//B_Hero_ShooterMannequin_C_0", "Test_DisableLookAt", new object[] { });
        }

        /// <summary>
        /// Teleport player character to position for shooting tests
        /// </summary>
        public static void TeleportToShootingTestLocation()
        {
            // Teleport Player character
            api.CharacterTeleportTo("B_Hero_ShooterMannequin_C_0", new Vector3(-935, -2133, 797));
            
            // Teleport bots
            api.CharacterTeleportTo("B_Hero_ShooterMannequin_C_1", new Vector3(-1061, -2964,797));
            api.CallMethod(
                "//B_Hero_ShooterMannequin_C_1",
                "Test_CharacterLookAt",
                new object[] { "B_Hero_ShooterMannequin_C_0" }
                );
            
            api.Wait(1000);
            api.CallMethod("//B_Hero_ShooterMannequin_C_1", "Test_DisableLookAt", new object[] { });
            api.Wait(1000);

            api.CharacterTeleportTo("B_Hero_ShooterMannequin_C_2", new Vector3(-856, -2964, 797));
            api.CallMethod(
                "//B_Hero_ShooterMannequin_C_2",
                "Test_CharacterLookAt",
                new object[] { "B_Hero_ShooterMannequin_C_0" }
                );

            api.Wait(1000);
            api.CallMethod("//B_Hero_ShooterMannequin_C_2", "Test_DisableLookAt", new object[] { });
            api.Wait(1000);

            api.CharacterTeleportTo("B_Hero_ShooterMannequin_C_3", new Vector3(-655, -2964, 797));
            api.CallMethod(
                "//B_Hero_ShooterMannequin_C_3",
                "Test_CharacterLookAt",
                new object[] { "B_Hero_ShooterMannequin_C_0" }
                );

            api.Wait(1000);
            api.CallMethod("//B_Hero_ShooterMannequin_C_3", "Test_DisableLookAt", new object[] { });
            api.Wait(1000);
        }

        /// <summary>
        /// Gets first character name from array by teamId that not equal to playerTeamId
        /// </summary>
        /// <param name="characters">Array of character names</param>
        /// <param name="playerTeamId">Current player teamId</param>
        /// <returns>Fisrt found character name from different team or empty string if nothig is found</returns>
        public static string GetFirstEnemyCharacterName(string[] characters, int playerTeamId)
        {
            foreach (var character in characters)
            {
                int characterTeamId = api.CallMethod<int>(
                    $"//{character}",
                    "Test_GetCharacterTeamID",
                    new object[] { character }
                    );

                api.Wait(1000);

                if (playerTeamId != characterTeamId)
                {
                    return character;
                }
            }
            return string.Empty;
        }

        #endregion
    }
}
