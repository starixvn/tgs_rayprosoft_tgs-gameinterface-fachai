namespace Tgs.GameInterface.FaChai.Core.UnitTest;

public class CryptographyUtensilUnitTest
{
	[Fact]
	public void AesDecrypt()
	{
		var cryptography = new CryptographyUtensil();

		var decrypt = cryptography.AesDecrypt("B8FafZBeSc8jZZv9DWnmQVfklLL/Yc5O0LCxzWb1h8UJe6j9xXfEArajXtPlWUpEA/wFiHFTcB3DpOAbwjn3AHHa2On3ELT86zz7/LdTj8U=", "iQ1mFWdzTh5zPO9r");

		Assert.NotNull(decrypt);
		Assert.Equal("{\"StartDate\":\"2019-11-01 00:00:00\",\"EndDate\":\"2019-11-30 23:59:59\"}", decrypt);
	}

	[Fact]
	public void AesEncrypt()
	{
		var cryptography = new CryptographyUtensil();

		var encrypt = cryptography.AesEncrypt("{\"StartDate\":\"2019-11-01 00:00:00\",\"EndDate\":\"2019-11-30 23:59:59\"}", "iQ1mFWdzTh5zPO9r");

		Assert.NotNull(encrypt);
		Assert.Equal("B8FafZBeSc8jZZv9DWnmQVfklLL/Yc5O0LCxzWb1h8UJe6j9xXfEArajXtPlWUpEA/wFiHFTcB3DpOAbwjn3AHHa2On3ELT86zz7/LdTj8U=", encrypt);
	}

	[Fact]
	public void Md5Encrypt()
	{
		var cryptography = new CryptographyUtensil();

		var encrypt = cryptography.Md5Encrypt("{\"StartDate\":\"2019-11-01 00:00:00\",\"EndDate\":\"2019-11-30 23:59:59\"}");

		Assert.NotNull(encrypt);
		Assert.Equal("0bbc20e189e27332ba4924743002b1ac", encrypt);
	}
}
