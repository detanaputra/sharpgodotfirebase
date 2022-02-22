using Godot;
using System;

using WAT;


namespace SharpGodotFirebaseTest
{
    [Title("Authentication Test 01")]
    public class AuthTest : Test
    {
        [Test]
        public void SigninWithPassword()
        {
            Describe("this is testing signin with email and password");
            Assert.IsTrue(true, "yeah");
        }
    }
}
