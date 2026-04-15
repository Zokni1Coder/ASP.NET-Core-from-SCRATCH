namespace Tests
{
    public class UnitTest1
    {
        [Fact] //Minden metódus ami a Fact attribútummal van ellátva, az teszt metódus.
        public void Test1()
        {
             //Ebbe a metódusba írjuk a saját unit tesztünket, akár többet is.

            //Minden unit test 3 lépésből áll: arrange, act, assert.
            //Arrange: változók deklarálására és az inputok begyüjtésére szolgál.
            //Act: ezzel jelöljük ki a futtatni/tesztelni kívánt metódust.
            //Assert: itt ellenőrizzük a kapott és a várt értéket.

            //Arrange:
            MyMath mm = new MyMath();
            int input1 = 10, input2 = 20, expected = 30;

            //Act:
            int actual = mm.Add(input1, input2);

            //Assert:
            //Mindig a várt érték az első param és az kapott a második.
            Assert.Equal(expected, actual);
        }        
    }
}
