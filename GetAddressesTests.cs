//-----------------------------------------------------------------------
// <copyright file="GetAddressesTests.cs" company="Procare Software, LLC">
//     Copyright © 2021-2023 Procare Software, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Procare.Address.IntegrationTests;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

#pragma warning disable SA1600 // Elements should be documented
public class GetAddressesTests
#pragma warning restore SA1600 // Elements should be documented
{
    private static readonly string[] Value =
    {
        "AL",
        "AK",
        "AZ",
        "AR",
        "CA",
        "CO",
        "CT",
        "DE",
        "DC",
        "FL",
        "GA",
        "HI",
        "ID",
        "IL",
        "IN",
        "IA",
        "KS",
        "KY",
        "LA",
        "ME",
        "MD",
        "MA",
        "MI",
        "MN",
        "MS",
        "MO",
        "MT",
        "NE",
        "NV",
        "NH",
        "NJ",
        "NM",
        "NY",
        "NC",
        "ND",
        "OH",
        "OK",
        "OR",
        "PA",
        "RI",
        "SC",
        "SD",
        "TN",
        "TX",
        "UT",
        "VT",
        "VA",
        "WA",
        "WV",
        "WI",
        "WY",
    };

    private static readonly string[] STATESUS =
    Value;

    private readonly AddressService service = new AddressService(new Uri("https://address.dev-procarepay.com"));

    /// <summary>
    /// GetAddresses_With_Owm_ShouldResultIn_OneMatchingAddress
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public async Task GetAddresses_With_Owm_ShouldResultIn_OneMatchingAddress()
    {
        var result = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "1 W Main St", City = "Medford", StateCode = "OR" }).ConfigureAwait(false);

        Assert.NotNull(result);
        Assert.Equal(1, result.Count);
        Assert.NotNull(result.Addresses);
        Assert.Equal(result.Count, result.Addresses!.Count);
    }

    /*
 *
 *         /*
     * Review the test solution to make sure you understand the test surface of the API. Feel free to ask questions.
     * What is a sub-set of requirements you think the business should check based on the description of the API?
     * Complete the started automated test validating that an ambiguous address will result in multiple matching valid shipping addresses.
     * Make a non-exhaustive list (not in code) of things to test based on the capabilities of the API & the above business requirements
     * (Hint: It only supports the US and Territories).
     * Pick two of those tests, preferably one positive and one negative, and add them to the automated integration tests.
     * Provide some thoughts on why you picked those two tests for automation.

 *I have povided 3 additional test cases, In first test case-GetAddresses_With_AmbiguousAddress_ShouldResultIn_MultipleMatchingAddresses- it wil validate that multiple objects are retreved when ambiguous
 * address is given , second test case-GetAddresses_With_AmbiguousAddress_Check_UsaAddress-  i have added validation for US territorry address
 * Third test case-GetAddresses_With_NonUsAddress_MultipleMatchingAddresses- In the input payload provided non us address and made sure that empty response is retreved by the API
 */

    /// <summary>
    /// GetAddresses_With_AmbiguousAddress_ShouldResultIn_MultipleMatchingAddress.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public async Task GetAddresses_With_AmbiguousAddress_ShouldResultIn_MultipleMatchingAddresses()
    {
        // Declare all USA States, here we can also use any open sourse liberary or package whic retreves the US states.
        // But i chose less dependent approarch
        try
        {
            var result = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "123 Main St", City = "Ontario", StateCode = "CA" }).ConfigureAwait(false);

            var addresses_list = result.Addresses;

            Assert.True(result.Count > 1); // verifying that it retrives more than 1 address
            Assert.NotNull(result.Addresses);
            Assert.Equal(result.Count, result.Addresses!.Count);
        }
        catch (Exception ex)
        {
            throw new NotImplementedException(ex);
        }
    }

    /// <summary>
    /// GetAddresses_With_AmbiguousAddress_Check_UsaAddress
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public async Task GetAddresses_With_AmbiguousAddress_Check_UsaAddress()
    {
        // Declare all USA States, here we can also use any open sourse liberary or package whic retreves the US states.
        // But i chose less dependent approarch
        try
        {
            var result = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "123 Main St", City = "Ontario", StateCode = "CA" }).ConfigureAwait(false);

            var addresses_list = result.Addresses;

            Assert.True(result.Count > 1); // verifying that it retrives more than 1 address
            Assert.NotNull(result.Addresses);
            Assert.Equal(result.Count, result.Addresses!.Count);

            // Checking whether the address is USA Addresss
            if (addresses_list is not null)
            {
                foreach (var address in addresses_list)
                {
                    foreach (var states in STATESUS)
                    {
                        if (address.StateCode != null && states.Contains(address.StateCode, System.StringComparison.CurrentCultureIgnoreCase))
                        {
                            Console.WriteLine(address.StateCode);
                            Assert.True(states.Contains(address.StateCode, System.StringComparison.CurrentCultureIgnoreCase));
                            break;
                        }
                    }

#pragma warning disable CS8604 // Possible null reference argument.
                    Assert.NotEmpty(address.City); // checking whether the city is non empty

                    Assert.NotEmpty(address.ZipCodeLeading5); // checking whethere ZipCodeLeading5 is empty
                    Assert.NotEmpty(address.ZipCodeTrailing4); // checking whether the ZipCodeLeading5 is non empty
#pragma warning restore CS8604 // Possible null reference argument.

                }
            }
        }
        catch (Exception ex)
        {
            throw new NotImplementedException(ex);
        }
    }

    /// <summary>
    /// GetAddresses_With_NonUsAddress_MultipleMatchingAddresses
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    [Fact]
    public async Task GetAddresses_With_NonUsAddress_MultipleMatchingAddresses()
    {
        try
        {
            var result = await this.service.GetAddressesAsync(new AddressFilter { Line1 = "123 Main St", City = "Ontario", StateCode = "UI" }).ConfigureAwait(false);

            Assert.True(result.Count == 0); // verifying that it retrives zero data
        }
        catch (Exception ex)
        {
            throw new NotImplementedException(ex);
        }
    }
}
