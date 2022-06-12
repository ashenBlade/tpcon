using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Router.Commands.TpLink.TLWR741ND.Configurators;
using Router.Commands.TpLink.TLWR741ND.Status.Wlan.Network;
using Router.Commands.TpLink.TLWR741ND.Status.Wlan.Security;
using Router.Commands.TpLink.TLWR741ND.Tests.Mocks;
using Router.Domain.Wlan;
using Router.Utils.Security;
using Xunit;

namespace Router.Commands.TpLink.TLWR741ND.Tests;

public class WlanConfiguratorTests
{
    private static WlanConfigurator CreateConfigurator(string? ssid = null,
                                                       bool enabled = true,
                                                       ChannelNumber? number = null,
                                                       ChannelWidth? width = null,
                                                       int rate = 150,
                                                       NetworkSpeedMeasurement measurement =
                                                           NetworkSpeedMeasurement.Mbps,
                                                       Security? security = null)
        => new(new MockRouterMessageSender(),
               new MockWlanNetworkRouterStatusExtractor(ssid, enabled, number, width, rate, measurement),
               new MockWlanSecurityRouterStatusExtractor(security ?? new NoneSecurity()));

    public static IEnumerable<string[]> ValidSSID =>
        new[]
        {
            new[] {"SSID"}, new[] {"valid ssid"}, new[] {"SSID"}, new[] {"SSID"}, new[] {"s"}, new[] {"WIFI"},
            new[] {" "}, new[] {"     "}, new[] {"12345678901234567890123456789012"},
            new[] {"                                "}, new[] {"  asfsadfsadfsdf "},
        };

    [Theory(DisplayName = "SetSSIDAsync; With valid SSID; Should set SSID")]
    [MemberData(nameof(ValidSSID))]
    public async Task SetSSID_WithValidSSID_ShouldSetSSID(string ssid)
    {
        var configurator = CreateConfigurator();
        var ex = await Record.ExceptionAsync(() => configurator.SetSSIDAsync(ssid));
        Assert.Null(ex);
    }

    [Fact(DisplayName = "SetSSIDAsync; With null SSID; Should throw exception")]
    public async Task SetSSIDAsync_WithNullSSID_ShouldThrowException()
    {
        var configurator = CreateConfigurator();
        await Assert.ThrowsAnyAsync<Exception>(() => configurator.SetSSIDAsync(null!));
    }

    [Fact(DisplayName = "SetSSIDAsync; With empty SSID; Should throw exception")]
    public async Task SetSSIDAsync_WithEmptySSID_ShouldThrowException()
    {
        var configurator = CreateConfigurator();
        await Assert.ThrowsAnyAsync<Exception>(() => configurator.SetSSIDAsync(string.Empty));
    }

    public static IEnumerable<string[]> LongSSID =>
        new[]
        {
            new[] {"123456789012345678901234567890123"}, new[] {"                                 "},
            new[] {"this_ssid_must_not_be_set_because_of_length"},
            new[] {"sadfasfsgsrhjehgnpaokjrgnpavjfporbhvanpkjpan"},
        };

    [Theory(DisplayName = "SetSSIDAsync; With valid SSID; Should set SSID")]
    [MemberData(nameof(LongSSID))]
    public async Task SetSSID_WithSSIDLongerThan32_ShouldThrowException(string ssid)
    {
        var configurator = CreateConfigurator();
        await Assert.ThrowsAnyAsync<Exception>(() => configurator.SetSSIDAsync(ssid));
    }

    private class AnotherSecurityType : Security
    {
        public override string Name => "Unsupported for this router type";
    }

    [Fact(DisplayName = "SetSecurityAsync; With unsupported security; Should throw exception")]
    public void SetSecurity_WithUnsupportedSecurity_ShouldThrowException()
    {
        var configurator = CreateConfigurator();
        Assert.ThrowsAnyAsync<Exception>(() => configurator.SetSecurityAsync(new AnotherSecurityType()));
    }
}