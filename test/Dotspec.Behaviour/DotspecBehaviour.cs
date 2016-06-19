using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Dotspec.Behaviour
{
    public class DotspecBehaviour
    {
        private void AssertScenario(string scenario, Action<Dotspec<object>> behaviour)
        {
            var subject = scenario.Spec<object>();

            behaviour(subject);
            subject.Assert(new object());
        }

        [Fact]
        public void ShouldStartWithPreconditionSpec()
        {
            var subject = "Should start with precondition spec".Spec<object>();
            var interfaces = subject.GetType().GetInterfaces();

            interfaces.Length.ShouldBe(1);
            interfaces[0].ShouldBeOfType<IPreconditionSpec<object>>();
        }

        [Fact]
        public void ShouldRegisterPrecondition()
        {
            var preconditionCalled = false;
            Action precondition = () => preconditionCalled = true;

            AssertScenario("Should register precondition", subject => subject.Given(precondition));
            preconditionCalled.ShouldBeTrue();
        }

        [Fact]
        public void ShouldRegisterAdditionalPreconditions()
        {
            var preconditionCalled = false;
            Action precondition = () => preconditionCalled = true;

            AssertScenario("Should register additional precondition", subject => subject.And(precondition));
            preconditionCalled.ShouldBeTrue();
        }
    }

    public enum ExchangeRateProviderEnum
    {
        Undefined = 0,

        Ofx = 1,

        Cba = 2,

        Nab = 3, 
        
        Anz = 4,

        Wbc = 5
    }

    public interface IExchangeRateChart
    {
        Dictionary<ExchangeRateProviderEnum, ExchangeRate> GetAllExchangeRates(string fromCurrency, string toCurrency);
    }

    public class ExchangeRateChart : IExchangeRateChart
    {
        private Dictionary<ExchangeRateProviderEnum, IExchangeRateProvider> ProviderMatrix { get; }

        public ExchangeRateChart(IExchangeRateProviderFactory providerFactory)
        {
            ProviderMatrix = new Dictionary<ExchangeRateProviderEnum, IExchangeRateProvider>
            {
                { ExchangeRateProviderEnum.Ofx, providerFactory.CreateExchangeRateProvider(ExchangeRateProviderEnum.Ofx) },
                { ExchangeRateProviderEnum.Cba, providerFactory.CreateExchangeRateProvider(ExchangeRateProviderEnum.Cba) }
            };
        }

        public Dictionary<ExchangeRateProviderEnum, ExchangeRate> GetAllExchangeRates(string fromCurrency, string toCurrency)
        {
            return ProviderMatrix.ToDictionary(kv => kv.Key, kv => kv.Value.GetRate(fromCurrency, toCurrency));
        }
    }

    public interface IExchangeRateProviderFactory
    {
        IExchangeRateProvider CreateExchangeRateProvider(ExchangeRateProviderEnum rateProvider);
    }

    public interface ILookup
    {
        T GetVal<T>(string key);
    }

    public class OfxExchangeRateProviderBehaviour
    {
        [Fact]
        public void ShouldGetRate()
        {
            Func<DateTime> now = () => new DateTime(2000, 1, 1);
            var parameters = Substitute.For<ILookup>();
            parameters.GetVal<decimal?>("buffer").Returns((decimal?)null);
            var fromCurrency = "AUD";
            var toCurrency = "HKD";
            var expectedRate = 2.5m;

            var subject = new OfxExchangeRateProvider(now, parameters);

            var result = subject.GetRate(fromCurrency, toCurrency);

            result.Date.ShouldBe(now());
            result.FromCurrency.ShouldBe(fromCurrency);
            result.ToCurrency.ShouldBe(toCurrency);
            result.Rate.ShouldBe(expectedRate);
        }

        [Fact]
        public void ShouldGetRateWithBuffer()
        {
            // Given
            var buffer = 2.5m;
            var testCase = SetupTestCase(3m, "AUD", "HKD", buffer);
            var subject = new OfxExchangeRateProvider(testCase["now"] as Func<DateTime>, testCase["parameters"] as ILookup);

            // When
            var result = subject.GetRate(testCase["fromCurrency"] as string, testCase["toCurrency"] as string);

            // Then
            AssertResult(testCase, result);
            result.Rate.ShouldBe((decimal)testCase["expectedRate"] + buffer);
        }

        [Fact]
        public void ShouldGetRateWithBufferWithDotSpec()
        {
            "Should get rate with buffer".Spec<OfxExchangeRateProvider>()
                .Given(
                    () => SetupTypedTestCase(3m, "AUD", "HKD", 2.5m))
                .When(
                    (subject, data) => subject.GetRate(data.FromCurrency, data.ToCurrency))
                .Assert(
                    data => new OfxExchangeRateProvider(data.Now, data.Parameters));
        }

        private void AssertResult(Dictionary<string, object> testCase, ExchangeRate result)
        {
            result.Date.ShouldBe((testCase["now"] as Func<DateTime>)());
            result.FromCurrency.ShouldBe(testCase["fromCurrency"] as string);
            result.ToCurrency.ShouldBe(testCase["toCurrency"] as string);
        }

        private Dictionary<string, object> SetupTestCase(decimal? buffer, string fromCurrency, string toCurrency, decimal? baseRate)
        {
            Func<DateTime> now = () => new DateTime(2000, 1, 1);
            var parameters = Substitute.For<ILookup>();
            parameters.GetVal<decimal?>("buffer").Returns(buffer);

            return new Dictionary<string, object>
            {
                { "now", now },
                { "parameters", parameters },
                { "fromCurrency", fromCurrency },
                { "toCurrency", toCurrency },
                { "baseRate", baseRate },
                { "expectedRate", buffer.HasValue ? baseRate + buffer.Value : baseRate }
            };
        }

        private TestCase SetupTypedTestCase(decimal? buffer, string fromCurrency, string toCurrency, decimal baseRate)
        {
            Func<DateTime> now = () => new DateTime(2000, 1, 1);
            var parameters = Substitute.For<ILookup>();
            parameters.GetVal<decimal?>("buffer").Returns(buffer);

            return new TestCase
            {
                Now = now,
                Parameters = parameters,
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency,
                BaseRate = baseRate,
                ExpectedRate = buffer.HasValue ? baseRate + buffer.Value : baseRate
            };
        }

        private class TestCase
        {
            public Func<DateTime> Now { get; set; }

            public ILookup Parameters { get; set; }

            public string FromCurrency { get; set; }

            public string ToCurrency { get; set; }

            public decimal BaseRate { get; set; }

            public decimal ExpectedRate { get; set; }
        }
    }

    public static class DictionaryExtension
    {
        public static T GetVal<T>(this Dictionary<string, object> dictionary, string key)
        {
            return (T)dictionary[key];
        }
    }

    public class OfxExchangeRateProvider : IExchangeRateProvider
    {
        private Func<DateTime> Now { get; }

        private ILookup Parameters { get; }
        
        public OfxExchangeRateProvider(Func<DateTime> now, ILookup parameters)
        {
            if (now == null) throw new ArgumentNullException(nameof(now));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            Now = now;
            Parameters = parameters;
        }

        public ExchangeRate GetRate(string fromCurrency, string toCurrency)
        {
            var rate = InvokeGetRate(fromCurrency, toCurrency);

            return new ExchangeRate
            {
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency,
                Rate = (decimal)rate[0],
                Date = (DateTime)rate[1]
            };
        }

        private object[] InvokeGetRate(string fromCurrency, string toCurrency)
        {
            var rate = 0m;

            if (string.Equals(fromCurrency, "AUD", StringComparison.InvariantCultureIgnoreCase))
            {
                rate = 2.5m;
            }
            else
            {
                rate = 1.3m;
            }

            var buffer = Parameters.GetVal<decimal?>("buffer");

            if (buffer.HasValue) rate += buffer.Value;

            return new object[]
            {
                rate,
                Now()
            };
        }
    }

    public class CbaExchangeRateProvider : IExchangeRateProvider
    {
        private Func<DateTime> Now { get; }

        public CbaExchangeRateProvider(Func<DateTime> now)
        {
            if (now == null) throw new ArgumentNullException(nameof(now));

            Now = now;
        }

        public ExchangeRate GetRate(string fromCurrency, string toCurrency)
        {
            var rate = InvokeGetRate(fromCurrency, toCurrency);

            return new ExchangeRate
            {
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency,
                Rate = (decimal)rate[0],
                Date = (DateTime)rate[1]
            };
        }

        private object[] InvokeGetRate(string fromCurrency, string toCurrency)
        {
            var rate = 0m;

            if (string.Equals(fromCurrency, "AUD", StringComparison.InvariantCultureIgnoreCase))
            {
                rate = 3.9m;
            }
            else
            {
                rate = 1.8m;
            }

            return new object[]
            {
                rate,
                Now()
            };
        }
    }

    public interface IExchangeRateProvider
    {
        ExchangeRate GetRate(string fromCurrency, string toCurrency);
    }

    public class ExchangeRate
    {
        public string FromCurrency { get; set; }

        public string ToCurrency { get; set; }

        public decimal Rate { get; set; }

        public DateTime Date { get; set; }
    }
}
