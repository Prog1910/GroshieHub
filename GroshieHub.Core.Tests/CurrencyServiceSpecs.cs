using GroshieHub.Public.Core.Entities;
using GroshieHub.Public.Core.Services;
using GroshieHub.Public.Shared.Enums;
using Microsoft.Extensions.Options;
using Moq;

namespace GroshieHub.Core.Tests;

public sealed class CurrencyServiceSpecs
{
	private static readonly string _currencyCode = CurrencyCode.USD.ToString();
	private static readonly Mock<ICurrencyClient> _clientMock = new();
	private static readonly Mock<IOptionsSnapshot<CurrencyApiSettings>> _optionsMock = new();

	[Theory]
	[ClassData(typeof(TestData))]
	public async Task Returns_correctly_formatted_default_currency(int roundCount, decimal rate, decimal expectedRate)
	{
		_clientMock
			.Setup(x => x.GetExchangeRateAsync(_currencyCode, null, It.IsAny<CancellationToken>()))
			.ReturnsAsync(rate);

		_optionsMock
			.Setup(x => x.Value)
			.Returns(new CurrencyApiSettings
			{
				BaseCurrencyCode = _currencyCode,
				DefaultCurrencyCode = _currencyCode,
				CurrencyRoundCount = roundCount
			});

		var currencyService = new CurrencyService(_clientMock.Object, _optionsMock.Object);

		var currencyDto = await currencyService.GetDefaultAsync();

		Assert.NotNull(currencyDto);
		Assert.Equal(_currencyCode, currencyDto.Code);
		Assert.Equal(expectedRate, currencyDto.Rate);
	}

	[Theory]
	[ClassData(typeof(TestData))]
	public async Task Returns_correctly_formatted_currency_by_code(int roundCount, decimal rate, decimal expectedRate)
	{
		_clientMock
			.Setup(x => x.GetExchangeRateAsync(_currencyCode, null, It.IsAny<CancellationToken>()))
			.ReturnsAsync(rate);

		_optionsMock
			.Setup(x => x.Value)
			.Returns(new CurrencyApiSettings
			{
				BaseCurrencyCode = _currencyCode,
				DefaultCurrencyCode = _currencyCode,
				CurrencyRoundCount = roundCount
			});

		var currencyService = new CurrencyService(_clientMock.Object, _optionsMock.Object);

		var currencyDto = await currencyService.GetByCodeAsync(_currencyCode);

		Assert.NotNull(currencyDto);
		Assert.Equal(_currencyCode, currencyDto.Code);
		Assert.Equal(expectedRate, currencyDto.Rate);
	}

	[Theory]
	[ClassData(typeof(TestData))]
	public async Task Returns_correctly_formatted_currency_on_date_by_code(int roundCount, decimal rate, decimal expectedRate)
	{
		var date = new DateTime(2000, 1, 5);
		var formattedDate = "2000-01-05";

		_clientMock
			.Setup(x => x.GetExchangeRateAsync(_currencyCode, date, It.IsAny<CancellationToken>()))
			.ReturnsAsync(rate);

		_optionsMock
			.Setup(x => x.Value)
			.Returns(new CurrencyApiSettings { CurrencyRoundCount = roundCount });

		var currencyService = new CurrencyService(_clientMock.Object, _optionsMock.Object);

		var currencyOnDateDto = await currencyService.GetByCodeOnDateAsync(_currencyCode, date);

		Assert.NotNull(currencyOnDateDto);
		Assert.Equal(_currencyCode, currencyOnDateDto.Code);
		Assert.Equal(formattedDate, currencyOnDateDto.Date);
		Assert.Equal(expectedRate, currencyOnDateDto.Rate);
	}

	[Fact]
	public async Task Returns_correct_settings()
	{
		short total = 100;
		short used = 1;
		var roundCount = 2;
		var baseCode = CurrencyCode.USD.ToString();
		var defaultCode = CurrencyCode.RUB.ToString();

		_clientMock
			.Setup(x => x.GetRequestLimitInfoAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync((total, used));

		_optionsMock
			.Setup(x => x.Value)
			.Returns(new CurrencyApiSettings
			{
				BaseCurrencyCode = baseCode,
				DefaultCurrencyCode = defaultCode,
				CurrencyRoundCount = roundCount
			});

		var currencyService = new CurrencyService(_clientMock.Object, _optionsMock.Object);

		var settingsDto = await currencyService.GetSettingsAsync();

		Assert.NotNull(settingsDto);
		Assert.Equal(total, settingsDto.RequestLimit);
		Assert.Equal(used, settingsDto.RequestCount);
		Assert.Equal(defaultCode, settingsDto.DefaultCurrencyCode);
		Assert.Equal(baseCode, settingsDto.BaseCurrencyCode);
		Assert.Equal(roundCount, settingsDto.CurrencyRoundCount);
	}

	private sealed class TestData : TheoryData<int, decimal, decimal>
	{
		public TestData()
		{
			Add(0, 1.12345m, 1m);
			Add(1, 1.12345m, 1.1m);
			Add(2, 1.12345m, 1.12m);
			Add(3, 1.12345m, 1.123m);
			Add(4, 1.12345m, 1.1234m);
			Add(5, 1.12345m, 1.12345m);
		}
	}
}