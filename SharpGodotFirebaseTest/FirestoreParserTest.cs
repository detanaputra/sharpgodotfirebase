using SharpGodotFirebase.Firestore;

using System;

using Xunit;
using Xunit.Abstractions;

namespace SharpGodotFirebaseTest
{
    public class FirestoreParserTest
    {
        private readonly ITestOutputHelper output;

        public FirestoreParserTest(ITestOutputHelper outputHelper)
        {
            output = outputHelper;
        }

        [Fact]
        public void Test1()
        {
            string json = @"
            {
                'name': 'projects/project-testing-b691b/databases/(default)/documents/FamilyCollection/LTCdKEWPkLZzC2YrYoNu',
                'fields': 
                 {
                    'FirstName': {
                        'stringValue': 'Deta'
                    },
                    'LastName': {
                        'stringValue': 'Novian Anantika Putra'
                    },
                    'Age': {
                        'integerValue': '31'
                    },
                    'TestMap': {
                        'mapValue': {
                            'fields': {
                                'MapKey2': {
                                    'stringValue': 'Test Map Value 2'
                                },
                                'MapKey1': {
                                    'stringValue': 'Test Map Value 1'
                                }
                            }
                        }
                    }
                },
                'createTime': '2022-02-19T01:36:18.228167Z',
                'updateTime': '2022-02-19T02:03:02.792716Z'
            }";
            object obj = FirestoreParser.Parse(json);
            output.WriteLine("yeah");
            Assert.True(true);

        }
    }
}