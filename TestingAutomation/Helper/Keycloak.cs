using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TestFramework.Contexts;

namespace TestFramework.Helper
{
    public class Keycloak
    {
        private HttpClient _httpClient;
        private readonly EnvironmentContext _environmentContext;

        public enum UserRole
        {
            Cashier,
            Supervisor
        }

        private static readonly string STORE_CASHIER_ROLE_NAME = "MPOS_STORE_CASHIER";
        private static readonly string STORE_SUPERVISOR_ROLE_NAME = "MPOS_STORE_SUPERVISOR";
        private static readonly string MPOS_AIR_UI_CLIENT_NAME = "MPOS_AIR_UI";

        public Keycloak(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _environmentContext = new EnvironmentContext();
        }

        private string GetKeycloakAdminUser() => _environmentContext.KeycloakConfig.AdminUserName;
        private string GetKeycloakAdminPassword() => _environmentContext.KeycloakConfig.AdminPassword;
        private string GetKeycloakRealmName() => "MPOS_REALM";
        private string GetKeycloakUrl() => _environmentContext.KeycloakConfig.URL;
        private JsonSerializerSettings GetJsonSerializerSettings() => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private class Token
        {
            public string Access_token { get; set; }
            public string Token_type { get; set; }
            public long Expires_in { get; set; }
        }

        public class KeycloakUser
        {
            public string Id { get; private set; }
            public int StoreNumber { get; private set; }
            public string OperatorId { get; private set; }
            public string Name { get; private set; }
            public string Barcode { get; private set; }
            public string Password { get; set; }
            public bool PasswordIsTemporary { get; set; }

            public KeycloakUser(string id, int storeNumber, string operatorId, string name, string barcode, bool passwordIsTemporary, string password)
            {
                Id = id;
                StoreNumber = storeNumber;
                OperatorId = operatorId;
                Name = name;
                Barcode = barcode;
                PasswordIsTemporary = passwordIsTemporary;
                Password = password;
            }
        }

        /// <summary>
        /// https://www.keycloak.org/docs-api/4.8/rest-api/#_credentialrepresentation
        /// </summary>
        private class KeycloakUserDto
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Username { get; set; }
            public bool Enabled { get; set; }

            public Dictionary<string, string> Attributes { get; set; }
        }

        /// <summary>
        /// https://www.keycloak.org/docs-api/4.8/rest-api/#_resetpassword
        /// </summary>
        public class KeycloakUserPasswordDto
        {
            public string Type { get; } = "password";
            public bool Temporary { get; set; } = false;
            public string Value { get; set; }

            public KeycloakUserPasswordDto(bool temporary, string value)
            {
                Temporary = temporary;
                Value = value;
            }
        }

        private class KeycloakRealmClientDto
        {
            public string Id { get; set; }
            public string ClientId { get; set; }
        }

        private class KeycloakRealmClientRoleDto
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        private class KeycloakUserRoleMappingDto
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public class CountryISO3
        {
            public int Numeric3 { get; set; }
            public string Alpha3 { get; set; }
            public string Name { get; set; }

            public CountryISO3(int numeric3, string alpha3, string name)
            {
                Numeric3 = numeric3;
                Alpha3 = alpha3;
                Name = name;
            }
        }

        /// <summary>
        /// https://www.keycloak.org/docs/4.8/authorization_services/#_service_overview
        /// </summary>
        /// <returns></returns>
        private async Task<Token> GetKeycloakAdminTokenAsync()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            try
            {
                var request = await _httpClient.PostAsync(
                    $"{GetKeycloakUrl()}/auth/realms/master/protocol/openid-connect/token",
                    new FormUrlEncodedContent(new Dictionary<string, string> {
                    {"username", GetKeycloakAdminUser()},
                    {"password", GetKeycloakAdminPassword()},
                    {"grant_type", "password"},
                    {"client_id","admin-cli"}
                    }));

                if (request.StatusCode == HttpStatusCode.OK)
                    return JsonConvert.DeserializeObject<Token>(await request.Content.ReadAsStringAsync());
            }
            catch
            {
                throw new Exception("Couldn't generate KeyCloak admin token");
            }

            return null;
        }

        private async Task<HttpClient> GetKeycloakAdminHttpClientAsync()
        {
            var adminToken = await GetKeycloakAdminTokenAsync();

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken.Access_token);

            return _httpClient;
        }

        #region ISO3166_1 list
        private static readonly IEnumerable<CountryISO3> ISO3166_1 = new List<CountryISO3>
        {
            new CountryISO3(4, "AFG", "Afghanistan"),
            new CountryISO3(8, "ALB", "Albania"),
            new CountryISO3(10, "ATA", "Antarctica"),
            new CountryISO3(12, "DZA", "Algeria"),
            new CountryISO3(16, "ASM", "American Samoa"),
            new CountryISO3(20, "AND", "Andorra"),
            new CountryISO3(24, "AGO", "Angola"),
            new CountryISO3(28, "ATG", "Antigua and Barbuda"),
            new CountryISO3(31, "AZE", "Azerbaijan"),
            new CountryISO3(32, "ARG", "Argentina"),
            new CountryISO3(36, "AUS", "Australia"),
            new CountryISO3(40, "AUT", "Austria"),
            new CountryISO3(44, "BHS", "Bahamas"),
            new CountryISO3(48, "BHR", "Bahrain"),
            new CountryISO3(50, "BGD", "Bangladesh"),
            new CountryISO3(51, "ARM", "Armenia"),
            new CountryISO3(52, "BRB", "Barbados"),
            new CountryISO3(56, "BEL", "Belgium"),
            new CountryISO3(60, "BMU", "Bermuda"),
            new CountryISO3(64, "BTN", "Bhutan"),
            new CountryISO3(68, "BOL", "Bolivia (Plurinational State of)"),
            new CountryISO3(70, "BIH", "Bosnia and Herzegovina"),
            new CountryISO3(72, "BWA", "Botswana"),
            new CountryISO3(74, "BVT", "Bouvet Island"),
            new CountryISO3(76, "BRA", "Brazil"),
            new CountryISO3(84, "BLZ", "Belize"),
            new CountryISO3(86, "IOT", "British Indian Ocean Territory"),
            new CountryISO3(90, "SLB", "Solomon Islands"),
            new CountryISO3(92, "VGB", "Virgin Islands (British)"),
            new CountryISO3(96, "BRN", "Brunei Darussalam"),
            new CountryISO3(100, "BGR", "Bulgaria"),
            new CountryISO3(104, "MMR", "Myanmar"),
            new CountryISO3(108, "BDI", "Burundi"),
            new CountryISO3(112, "BLR", "Belarus"),
            new CountryISO3(116, "KHM", "Cambodia"),
            new CountryISO3(120, "CMR", "Cameroon"),
            new CountryISO3(124, "CAN", "Canada"),
            new CountryISO3(132, "CPV", "Cabo Verde"),
            new CountryISO3(136, "CYM", "Cayman Islands"),
            new CountryISO3(140, "CAF", "Central African Republic"),
            new CountryISO3(144, "LKA", "Sri Lanka"),
            new CountryISO3(148, "TCD", "Chad"),
            new CountryISO3(152, "CHL", "Chile"),
            new CountryISO3(156, "CHN", "China"),
            new CountryISO3(158, "TWN", "Taiwan, Province of China"),
            new CountryISO3(162, "CXR", "Christmas Island"),
            new CountryISO3(166, "CCK", "Cocos (Keeling) Islands"),
            new CountryISO3(170, "COL", "Colombia"),
            new CountryISO3(174, "COM", "Comoros"),
            new CountryISO3(175, "MYT", "Mayotte"),
            new CountryISO3(178, "COG", "Congo"),
            new CountryISO3(180, "COD", "Congo, Democratic Republic of the"),
            new CountryISO3(184, "COK", "Cook Islands"),
            new CountryISO3(188, "CRI", "Costa Rica"),
            new CountryISO3(191, "HRV", "Croatia"),
            new CountryISO3(192, "CUB", "Cuba"),
            new CountryISO3(196, "CYP", "Cyprus"),
            new CountryISO3(203, "CZE", "Czechia"),
            new CountryISO3(204, "BEN", "Benin"),
            new CountryISO3(208, "DNK", "Denmark"),
            new CountryISO3(212, "DMA", "Dominica"),
            new CountryISO3(214, "DOM", "Dominican Republic"),
            new CountryISO3(218, "ECU", "Ecuador"),
            new CountryISO3(222, "SLV", "El Salvador"),
            new CountryISO3(226, "GNQ", "Equatorial Guinea"),
            new CountryISO3(231, "ETH", "Ethiopia"),
            new CountryISO3(232, "ERI", "Eritrea"),
            new CountryISO3(233, "EST", "Estonia"),
            new CountryISO3(234, "FRO", "Faroe Islands"),
            new CountryISO3(238, "FLK", "Falkland Islands (Malvinas)"),
            new CountryISO3(239, "SGS", "South Georgia and the South Sandwich Islands"),
            new CountryISO3(242, "FJI", "Fiji"),
            new CountryISO3(246, "FIN", "Finland"),
            new CountryISO3(248, "ALA", "Åland Islands"),
            new CountryISO3(250, "FRA", "France"),
            new CountryISO3(254, "GUF", "French Guiana"),
            new CountryISO3(258, "PYF", "French Polynesia"),
            new CountryISO3(260, "ATF", "French Southern Territories"),
            new CountryISO3(262, "DJI", "Djibouti"),
            new CountryISO3(266, "GAB", "Gabon"),
            new CountryISO3(268, "GEO", "Georgia"),
            new CountryISO3(270, "GMB", "Gambia"),
            new CountryISO3(275, "PSE", "Palestine, State of"),
            new CountryISO3(276, "DEU", "Germany"),
            new CountryISO3(288, "GHA", "Ghana"),
            new CountryISO3(292, "GIB", "Gibraltar"),
            new CountryISO3(296, "KIR", "Kiribati"),
            new CountryISO3(300, "GRC", "Greece"),
            new CountryISO3(304, "GRL", "Greenland"),
            new CountryISO3(308, "GRD", "Grenada"),
            new CountryISO3(312, "GLP", "Guadeloupe"),
            new CountryISO3(316, "GUM", "Guam"),
            new CountryISO3(320, "GTM", "Guatemala"),
            new CountryISO3(324, "GIN", "Guinea"),
            new CountryISO3(328, "GUY", "Guyana"),
            new CountryISO3(332, "HTI", "Haiti"),
            new CountryISO3(334, "HMD", "Heard Island and McDonald Islands"),
            new CountryISO3(336, "VAT", "Holy See"),
            new CountryISO3(340, "HND", "Honduras"),
            new CountryISO3(344, "HKG", "Hong Kong"),
            new CountryISO3(348, "HUN", "Hungary"),
            new CountryISO3(352, "ISL", "Iceland"),
            new CountryISO3(356, "IND", "India"),
            new CountryISO3(360, "IDN", "Indonesia"),
            new CountryISO3(364, "IRN", "Iran (Islamic Republic of)"),
            new CountryISO3(368, "IRQ", "Iraq"),
            new CountryISO3(372, "IRL", "Ireland"),
            new CountryISO3(376, "ISR", "Israel"),
            new CountryISO3(380, "ITA", "Italy"),
            new CountryISO3(384, "CIV", "Côte d'Ivoire"),
            new CountryISO3(388, "JAM", "Jamaica"),
            new CountryISO3(392, "JPN", "Japan"),
            new CountryISO3(398, "KAZ", "Kazakhstan"),
            new CountryISO3(400, "JOR", "Jordan"),
            new CountryISO3(404, "KEN", "Kenya"),
            new CountryISO3(408, "PRK", "Korea (Democratic People's Republic of)"),
            new CountryISO3(410, "KOR", "Korea, Republic of"),
            new CountryISO3(414, "KWT", "Kuwait"),
            new CountryISO3(417, "KGZ", "Kyrgyzstan"),
            new CountryISO3(418, "LAO", "Lao People's Democratic Republic"),
            new CountryISO3(422, "LBN", "Lebanon"),
            new CountryISO3(426, "LSO", "Lesotho"),
            new CountryISO3(428, "LVA", "Latvia"),
            new CountryISO3(430, "LBR", "Liberia"),
            new CountryISO3(434, "LBY", "Libya"),
            new CountryISO3(438, "LIE", "Liechtenstein"),
            new CountryISO3(440, "LTU", "Lithuania"),
            new CountryISO3(442, "LUX", "Luxembourg"),
            new CountryISO3(446, "MAC", "Macao"),
            new CountryISO3(450, "MDG", "Madagascar"),
            new CountryISO3(454, "MWI", "Malawi"),
            new CountryISO3(458, "MYS", "Malaysia"),
            new CountryISO3(462, "MDV", "Maldives"),
            new CountryISO3(466, "MLI", "Mali"),
            new CountryISO3(470, "MLT", "Malta"),
            new CountryISO3(474, "MTQ", "Martinique"),
            new CountryISO3(478, "MRT", "Mauritania"),
            new CountryISO3(480, "MUS", "Mauritius"),
            new CountryISO3(484, "MEX", "Mexico"),
            new CountryISO3(492, "MCO", "Monaco"),
            new CountryISO3(496, "MNG", "Mongolia"),
            new CountryISO3(498, "MDA", "Moldova, Republic of"),
            new CountryISO3(499, "MNE", "Montenegro"),
            new CountryISO3(500, "MSR", "Montserrat"),
            new CountryISO3(504, "MAR", "Morocco"),
            new CountryISO3(508, "MOZ", "Mozambique"),
            new CountryISO3(512, "OMN", "Oman"),
            new CountryISO3(516, "NAM", "Namibia"),
            new CountryISO3(520, "NRU", "Nauru"),
            new CountryISO3(524, "NPL", "Nepal"),
            new CountryISO3(528, "NLD", "Netherlands"),
            new CountryISO3(531, "CUW", "Curaçao"),
            new CountryISO3(533, "ABW", "Aruba"),
            new CountryISO3(534, "SXM", "Sint Maarten (Dutch part)"),
            new CountryISO3(535, "BES", "Bonaire, Sint Eustatius and Saba"),
            new CountryISO3(540, "NCL", "New Caledonia"),
            new CountryISO3(548, "VUT", "Vanuatu"),
            new CountryISO3(554, "NZL", "New Zealand"),
            new CountryISO3(558, "NIC", "Nicaragua"),
            new CountryISO3(562, "NER", "Niger"),
            new CountryISO3(566, "NGA", "Nigeria"),
            new CountryISO3(570, "NIU", "Niue"),
            new CountryISO3(574, "NFK", "Norfolk Island"),
            new CountryISO3(578, "NOR", "Norway"),
            new CountryISO3(580, "MNP", "Northern Mariana Islands"),
            new CountryISO3(581, "UMI", "United States Minor Outlying Islands"),
            new CountryISO3(583, "FSM", "Micronesia (Federated States of)"),
            new CountryISO3(584, "MHL", "Marshall Islands"),
            new CountryISO3(585, "PLW", "Palau"),
            new CountryISO3(586, "PAK", "Pakistan"),
            new CountryISO3(591, "PAN", "Panama"),
            new CountryISO3(598, "PNG", "Papua New Guinea"),
            new CountryISO3(600, "PRY", "Paraguay"),
            new CountryISO3(604, "PER", "Peru"),
            new CountryISO3(608, "PHL", "Philippines"),
            new CountryISO3(612, "PCN", "Pitcairn"),
            new CountryISO3(616, "POL", "Poland"),
            new CountryISO3(620, "PRT", "Portugal"),
            new CountryISO3(624, "GNB", "Guinea-Bissau"),
            new CountryISO3(626, "TLS", "Timor-Leste"),
            new CountryISO3(630, "PRI", "Puerto Rico"),
            new CountryISO3(634, "QAT", "Qatar"),
            new CountryISO3(638, "REU", "Réunion"),
            new CountryISO3(642, "ROU", "Romania"),
            new CountryISO3(643, "RUS", "Russian Federation"),
            new CountryISO3(646, "RWA", "Rwanda"),
            new CountryISO3(652, "BLM", "Saint Barthélemy"),
            new CountryISO3(654, "SHN", "Saint Helena, Ascension and Tristan da Cunha"),
            new CountryISO3(659, "KNA", "Saint Kitts and Nevis"),
            new CountryISO3(660, "AIA", "Anguilla"),
            new CountryISO3(662, "LCA", "Saint Lucia"),
            new CountryISO3(663, "MAF", "Saint Martin (French part)"),
            new CountryISO3(666, "SPM", "Saint Pierre and Miquelon"),
            new CountryISO3(670, "VCT", "Saint Vincent and the Grenadines"),
            new CountryISO3(674, "SMR", "San Marino"),
            new CountryISO3(678, "STP", "Sao Tome and Principe"),
            new CountryISO3(682, "SAU", "Saudi Arabia"),
            new CountryISO3(686, "SEN", "Senegal"),
            new CountryISO3(688, "SRB", "Serbia"),
            new CountryISO3(690, "SYC", "Seychelles"),
            new CountryISO3(694, "SLE", "Sierra Leone"),
            new CountryISO3(702, "SGP", "Singapore"),
            new CountryISO3(703, "SVK", "Slovakia"),
            new CountryISO3(704, "VNM", "Viet Nam"),
            new CountryISO3(705, "SVN", "Slovenia"),
            new CountryISO3(706, "SOM", "Somalia"),
            new CountryISO3(710, "ZAF", "South Africa"),
            new CountryISO3(716, "ZWE", "Zimbabwe"),
            new CountryISO3(724, "ESP", "Spain"),
            new CountryISO3(728, "SSD", "South Sudan"),
            new CountryISO3(729, "SDN", "Sudan"),
            new CountryISO3(732, "ESH", "Western Sahara"),
            new CountryISO3(740, "SUR", "Suriname"),
            new CountryISO3(744, "SJM", "Svalbard and Jan Mayen"),
            new CountryISO3(748, "SWZ", "Eswatini"),
            new CountryISO3(752, "SWE", "Sweden"),
            new CountryISO3(756, "CHE", "Switzerland"),
            new CountryISO3(760, "SYR", "Syrian Arab Republic"),
            new CountryISO3(762, "TJK", "Tajikistan"),
            new CountryISO3(764, "THA", "Thailand"),
            new CountryISO3(768, "TGO", "Togo"),
            new CountryISO3(772, "TKL", "Tokelau"),
            new CountryISO3(776, "TON", "Tonga"),
            new CountryISO3(780, "TTO", "Trinidad and Tobago"),
            new CountryISO3(784, "ARE", "United Arab Emirates"),
            new CountryISO3(788, "TUN", "Tunisia"),
            new CountryISO3(792, "TUR", "Turkey"),
            new CountryISO3(795, "TKM", "Turkmenistan"),
            new CountryISO3(796, "TCA", "Turks and Caicos Islands"),
            new CountryISO3(798, "TUV", "Tuvalu"),
            new CountryISO3(800, "UGA", "Uganda"),
            new CountryISO3(804, "UKR", "Ukraine"),
            new CountryISO3(807, "MKD", "North Macedonia"),
            new CountryISO3(818, "EGY", "Egypt"),
            new CountryISO3(826, "GBR", "United Kingdom of Great Britain and Northern Ireland"),
            new CountryISO3(831, "GGY", "Guernsey"),
            new CountryISO3(832, "JEY", "Jersey"),
            new CountryISO3(833, "IMN", "Isle of Man"),
            new CountryISO3(834, "TZA", "Tanzania, United Republic of"),
            new CountryISO3(840, "USA", "United States of America"),
            new CountryISO3(850, "VIR", "Virgin Islands (U.S.)"),
            new CountryISO3(854, "BFA", "Burkina Faso"),
            new CountryISO3(858, "URY", "Uruguay"),
            new CountryISO3(860, "UZB", "Uzbekistan"),
            new CountryISO3(862, "VEN", "Venezuela (Bolivarian Republic of)"),
            new CountryISO3(876, "WLF", "Wallis and Futuna"),
            new CountryISO3(882, "WSM", "Samoa"),
            new CountryISO3(887, "YEM", "Yemen"),
            new CountryISO3(894, "ZMB", "Zambia")
        };
        #endregion

        private static string ConvertToISO3166_1_Numeric(string countryIso3) =>
            ISO3166_1.First(i => i.Alpha3.Equals(countryIso3, StringComparison.OrdinalIgnoreCase)).Numeric3.ToString("000");

        private static string ConvertToDistributionLine(int distributionLine) =>
            distributionLine > 99 ? $"{distributionLine}".Substring(0, 2) : distributionLine.ToString("00");

        private static string ConvertToStoreNumber(int storeNumber) =>
            storeNumber > 999 ? $"{storeNumber}".Substring(0, 3) : storeNumber.ToString("000");

        private static string ConvertToCustomerNumber(string customerNumber) =>
            customerNumber.PadLeft(8, '0');

        private static string ConvertToCardHolder(int cardHolder) =>
            cardHolder > 99 ? $"{cardHolder}".Substring(0, 2) : cardHolder.ToString("00");

        private static string ConvertToCardVersion(int cardVersion) =>
            $"{cardVersion}".Substring(0, 1);

        private static string ConvertToBranch(int branch) =>
            $"{branch}".Substring(0, 1);

        private static int Mod10(string barcode)
        {
            var sum = 0;
            var weight = 2;
            for (var i = barcode.Length - 1; i >= 0; i += -1)
            {
                var result = (int)char.GetNumericValue(barcode[i]) * weight;
                if (result > 9)
                    result = (int)(Math.Floor((decimal)result / 10)) + (result % 10);
                sum += result;
                weight = weight == 2 ? 1 : 2;
            }
            var checkDigit = 10 - (int)sum % 10;
            return checkDigit == 10 ? 0 : checkDigit;
        }

        private static int Mod22(string barcode)
        {
            var sum = 0;
            var weight = 2;
            for (var i = barcode.Length - 1; i >= 0; i--)
            {
                sum += ((int)char.GetNumericValue(barcode[i]) * weight++);
                if (weight > 7)
                    weight = 2;
            }
            var checkDigit = 11 - (sum % 11);
            return checkDigit > 9 ? 0 : checkDigit;
        }

        public static string GenerateBarcode(
           string username,
           int storeNumber = 1,
           string countryIso3 = "ESP",
           int distributionLine = 0,
           int cardHolder = 1,
           int cardVersion = 1,
           int branch = 0)
        {
            var barcode =
                $"{ConvertToISO3166_1_Numeric(countryIso3)}" +
                $"{ConvertToDistributionLine(distributionLine)}" +
                $"{ConvertToStoreNumber(storeNumber)}" +
                $"{ConvertToCustomerNumber(username)}" +
                $"{ConvertToCardHolder(cardHolder)}" +
                $"{ConvertToCardVersion(cardVersion)}" +
                $"{ConvertToBranch(branch)}";

            barcode = $"{barcode}{Mod10(barcode)}";

            return $"{barcode}{Mod22(barcode)}";
        }

        public static string GenerateBarcode(
            KeycloakUser keycloakUser,
            int storeNumber = 1,
            string countryIso3 = "ESP",
            int distributionLine = 0,
            int cardHolder = 1,
            int cardVersion = 1,
            int branch = 0) =>
        GenerateBarcode(keycloakUser.Name, storeNumber, countryIso3, distributionLine, cardHolder, cardVersion, branch);

        /// <summary>
        /// https://www.keycloak.org/docs-api/4.8/rest-api/#_users_resource
        /// </summary>
        /// <returns></returns>
        public async Task<KeycloakUser> CreateKeycloakUserAsync(
            int storeNumber,
            UserRole userRole,
            bool useTemporaryPassword,
            string password)
        {
            var client = await GetKeycloakAdminHttpClientAsync();
            var operatorId = Utils.RandomDigits(4);
            var user = new KeycloakUserDto
            {
                FirstName = $"autogenerated",
                Username = $"7{Utils.RandomDigits(5)}",
                Enabled = true,
                Attributes = new Dictionary<string, string>
                {
                    { "operatorId", operatorId },
                    { "storeId", $"{storeNumber}" }
                }
            };

            var barcode = GenerateBarcode(user.Username, storeNumber: storeNumber);
            user.LastName = barcode;

            var createUserRequest = await client.PostAsync(
                $"{GetKeycloakUrl()}/auth/admin/realms/{GetKeycloakRealmName()}/users",
                new StringContent(
                    JsonConvert.SerializeObject(user, GetJsonSerializerSettings()),
                    Encoding.UTF8,
                    "application/json"));

            if (createUserRequest.StatusCode != HttpStatusCode.Created)
                throw new Exception("Couldn't create Keycloak user");

            var userId = createUserRequest.Headers.Location.Segments.Last();

            var userPassword = new KeycloakUserPasswordDto(useTemporaryPassword, password);

            //  https://www.keycloak.org/docs-api/4.8/rest-api/#_resetpassword
            var createUserPasswordRequest = await client.PutAsync(
                $"{GetKeycloakUrl()}/auth/admin/realms/{GetKeycloakRealmName()}/users/{userId}/reset-password",
                new StringContent(
                    JsonConvert.SerializeObject(userPassword, GetJsonSerializerSettings()),
                    Encoding.UTF8,
                    "application/json"));

            if (createUserPasswordRequest.StatusCode != HttpStatusCode.NoContent)
                throw new Exception("Couldn't create Keycloak user password");

            //  https://www.keycloak.org/docs-api/5.0/rest-api/index.html#_clients_resource
            var getRealmClientsRequest = await client.GetAsync($"{GetKeycloakUrl()}/auth/admin/realms/{GetKeycloakRealmName()}/clients");
            if (getRealmClientsRequest.StatusCode != HttpStatusCode.OK)
                throw new Exception("Couldn't fetch realm clients");

            var realmClients = JsonConvert.DeserializeObject<List<KeycloakRealmClientDto>>(
                await getRealmClientsRequest.Content.ReadAsStringAsync());

            var mposAirUiClient = realmClients.FirstOrDefault(r => r.ClientId.Equals(MPOS_AIR_UI_CLIENT_NAME, StringComparison.OrdinalIgnoreCase));
            if (mposAirUiClient == null)
                throw new Exception($"Client {MPOS_AIR_UI_CLIENT_NAME} not found!");

            //  https://www.keycloak.org/docs-api/5.0/rest-api/index.html#_roles_resource
            var getClientRolesRequest = await client.GetAsync($"{GetKeycloakUrl()}/auth/admin/realms/{GetKeycloakRealmName()}/clients/{mposAirUiClient.Id}/roles");
            if (getClientRolesRequest.StatusCode != HttpStatusCode.OK)
                throw new Exception("Couldn't fetch realm client roles");

            var clientRoles = JsonConvert.DeserializeObject<List<KeycloakRealmClientRoleDto>>(
                await getClientRolesRequest.Content.ReadAsStringAsync());

            var role = "";
            role = userRole switch
            {
                UserRole.Cashier => STORE_CASHIER_ROLE_NAME,
                UserRole.Supervisor => STORE_SUPERVISOR_ROLE_NAME,
                _ => STORE_CASHIER_ROLE_NAME,
            };

            var createRole = clientRoles.FirstOrDefault(r => r.Name.Equals(role, StringComparison.OrdinalIgnoreCase));
            if (createRole == null)
                throw new Exception($"Role {role} not found!");

            //  https://www.keycloak.org/docs-api/5.0/rest-api/index.html#_addclientrolemapping
            var userRoles = new List<KeycloakRealmClientRoleDto>
            {
                new KeycloakRealmClientRoleDto { Id = createRole.Id, Name = createRole.Name}
            };

            var assignRoleRequest = await client.PostAsync(
                $"{GetKeycloakUrl()}/auth/admin/realms/{GetKeycloakRealmName()}/users/{userId}/role-mappings/clients/{mposAirUiClient.Id}",
                new StringContent(
                    JsonConvert.SerializeObject(userRoles, GetJsonSerializerSettings()),
                    Encoding.UTF8,
                    "application/json"));

            if (assignRoleRequest.StatusCode != HttpStatusCode.NoContent)
                throw new Exception($"Couldn't assign {role} role to user");

            return new KeycloakUser(userId, storeNumber, operatorId, user.Username, barcode, userPassword.Temporary, userPassword.Value);
        }

        /// <summary>
        /// https://www.keycloak.org/docs-api/4.8/rest-api/#_deleteuser
        /// </summary>
        /// <param name="keycloakUser"></param>
        /// <returns></returns>
        public async Task DeleteKeyCloakUserAsync(KeycloakUser keycloakUser)
        {
            if (keycloakUser == null || string.IsNullOrEmpty(keycloakUser.Id))
                return;

            var client = await GetKeycloakAdminHttpClientAsync();
            await client.DeleteAsync($"{GetKeycloakUrl()}/auth/admin/realms/{GetKeycloakRealmName()}/users/{keycloakUser.Id}");
        }

        public enum OperatorTypeAuth
        {
            Cashier,
            Supervisor
        }
    }
}
