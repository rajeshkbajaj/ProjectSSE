using Serilog;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Covidien.CGRS.ESS
{
    public partial class DeviceCountryControl : UserControl
    {
        private readonly SortedDictionary<string, string> CountryDictionary = new SortedDictionary<string, string>();
        public event EventHandler DeviceCountrySubmitDone;

        public DeviceCountryControl()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            InitializeCountryCodes();
            DeviceCountrySubmitButton.Enabled = false;
        }

        private void InitializeCountryCodes()
        {
            Log.Information($"DeviceCountryControl:InitializeCountryCodes Entry");
            CountryDictionary.Add("Andorra", "AD");
            CountryDictionary.Add("United Arab Emirates", "AE");
            CountryDictionary.Add("Afghanistan", "AF");
            CountryDictionary.Add("Antigua and Barbuda", "AG");
            CountryDictionary.Add("Anguilla", "AI");
            CountryDictionary.Add("Albania", "AL");
            CountryDictionary.Add("Armenia", "AM");
            CountryDictionary.Add("Angola", "AO");
            CountryDictionary.Add("Antarctica", "AQ");
            CountryDictionary.Add("Argentina", "AR");
            CountryDictionary.Add("American Samoa", "AS");
            CountryDictionary.Add("Austria", "AT");
            CountryDictionary.Add("Australia", "AU");
            CountryDictionary.Add("Aruba", "AW");
            CountryDictionary.Add("Åland Islands", "AX");
            CountryDictionary.Add("Azerbaijan", "AZ");
            CountryDictionary.Add("Bosnia and Herzegovina", "BA");
            CountryDictionary.Add("Barbados", "BB");
            CountryDictionary.Add("Bangladesh", "BD");
            CountryDictionary.Add("Belgium", "BE");
            CountryDictionary.Add("Burkina Faso", "BF");
            CountryDictionary.Add("Bulgaria", "BG");
            CountryDictionary.Add("Bahrain", "BH");
            CountryDictionary.Add("Burundi", "BI");
            CountryDictionary.Add("Benin", "BJ");
            CountryDictionary.Add("Saint Barthélemy", "BL");
            CountryDictionary.Add("Bermuda", "BM");
            CountryDictionary.Add("Brunei Darussalam", "BN");
            CountryDictionary.Add("Bolivia, Plurinational State of", "BO");
            CountryDictionary.Add("Bonaire, Sint Eustatius and Saba", "BQ");
            CountryDictionary.Add("Brazil", "BR");
            CountryDictionary.Add("Bahamas", "BS");
            CountryDictionary.Add("Bhutan", "BT");
            CountryDictionary.Add("Bouvet Island", "BV");
            CountryDictionary.Add("Botswana", "BW");
            CountryDictionary.Add("Belarus", "BY");
            CountryDictionary.Add("Belize", "BZ");
            CountryDictionary.Add("Canada", "CA");
            CountryDictionary.Add("Cocos (Keeling) Islands", "CC");
            CountryDictionary.Add("Congo, the Democratic Republic of the", "CD");
            CountryDictionary.Add("Central African Republic", "CF");
            CountryDictionary.Add("Congo", "CG");
            CountryDictionary.Add("Switzerland", "CH");
            CountryDictionary.Add("Côte d'Ivoire", "CI");
            CountryDictionary.Add("Cook Islands", "CK");
            CountryDictionary.Add("Chile", "CL");
            CountryDictionary.Add("Cameroon", "CM");
            CountryDictionary.Add("China", "CN");
            CountryDictionary.Add("Colombia", "CO");
            CountryDictionary.Add("Costa Rica", "CR");
            CountryDictionary.Add("Cuba", "CU");
            CountryDictionary.Add("Cape Verde", "CV");
            CountryDictionary.Add("Curaçao", "CW");
            CountryDictionary.Add("Christmas Island", "CX");
            CountryDictionary.Add("Cyprus", "CY");
            CountryDictionary.Add("Czech Republic", "CZ");
            CountryDictionary.Add("Germany", "DE");
            CountryDictionary.Add("Djibouti", "DJ");
            CountryDictionary.Add("Denmark", "DK");
            CountryDictionary.Add("Dominica", "DM");
            CountryDictionary.Add("Dominican Republic", "DO");
            CountryDictionary.Add("Algeria", "DZ");
            CountryDictionary.Add("Ecuador", "EC");
            CountryDictionary.Add("Estonia", "EE");
            CountryDictionary.Add("Egypt", "EG");
            CountryDictionary.Add("Western Sahara", "EH");
            CountryDictionary.Add("Eritrea", "ER");
            CountryDictionary.Add("Spain", "ES");
            CountryDictionary.Add("Ethiopia", "ET");
            CountryDictionary.Add("Finland", "FI");
            CountryDictionary.Add("Fiji", "FJ");
            CountryDictionary.Add("Falkland Islands (Malvinas)", "FK");
            CountryDictionary.Add("Micronesia, Federated States of", "FM");
            CountryDictionary.Add("Faroe Islands", "FO");
            CountryDictionary.Add("France", "FR");
            CountryDictionary.Add("Gabon", "GA");
            CountryDictionary.Add("United Kingdom", "GB");
            CountryDictionary.Add("Grenada", "GD");
            CountryDictionary.Add("Georgia", "GE");
            CountryDictionary.Add("French Guiana", "GF");
            CountryDictionary.Add("Guernsey", "GG");
            CountryDictionary.Add("Ghana", "GH");
            CountryDictionary.Add("Gibraltar", "GI");
            CountryDictionary.Add("Greenland", "GL");
            CountryDictionary.Add("Gambia", "GM");
            CountryDictionary.Add("Guinea", "GN");
            CountryDictionary.Add("Guadeloupe", "GP");
            CountryDictionary.Add("Equatorial Guinea", "GQ");
            CountryDictionary.Add("Greece", "GR");
            CountryDictionary.Add("South Georgia and the South Sandwich Islands", "GS");
            CountryDictionary.Add("Guatemala", "GT");
            CountryDictionary.Add("Guam", "GU");
            CountryDictionary.Add("Guinea-Bissau", "GW");
            CountryDictionary.Add("Guyana", "GY");
            CountryDictionary.Add("Hong Kong", "HK");
            CountryDictionary.Add("Heard Island and McDonald Islands", "HM");
            CountryDictionary.Add("Honduras", "HN");
            CountryDictionary.Add("Croatia", "HR");
            CountryDictionary.Add("Haiti", "HT");
            CountryDictionary.Add("Hungary", "HU");
            CountryDictionary.Add("Indonesia", "ID");
            CountryDictionary.Add("Ireland", "IE");
            CountryDictionary.Add("Israel", "IL");
            CountryDictionary.Add("Isle of Man", "IM");
            CountryDictionary.Add("India", "IN");
            CountryDictionary.Add("British Indian Ocean Territory", "IO");
            CountryDictionary.Add("Iraq", "IQ");
            CountryDictionary.Add("Iran, Islamic Republic of", "IR");
            CountryDictionary.Add("Iceland", "IS");
            CountryDictionary.Add("Italy", "IT");
            CountryDictionary.Add("Jersey", "JE");
            CountryDictionary.Add("Jamaica", "JM");
            CountryDictionary.Add("Jordan", "JO");
            CountryDictionary.Add("Japan", "JP");
            CountryDictionary.Add("Kenya", "KE");
            CountryDictionary.Add("Kyrgyzstan", "KG");
            CountryDictionary.Add("Cambodia", "KH");
            CountryDictionary.Add("Kiribati", "KI");
            CountryDictionary.Add("Comoros", "KM");
            CountryDictionary.Add("Saint Kitts and Nevis", "KN");
            CountryDictionary.Add("Korea, Democratic People's Republic of", "KP");
            CountryDictionary.Add("Korea, Republic of", "KR");
            CountryDictionary.Add("Kuwait", "KW");
            CountryDictionary.Add("Cayman Islands", "KY");
            CountryDictionary.Add("Kazakhstan", "KZ");
            CountryDictionary.Add("Lao People's Democratic Republic", "LA");
            CountryDictionary.Add("Lebanon", "LB");
            CountryDictionary.Add("Saint Lucia", "LC");
            CountryDictionary.Add("Liechtenstein", "LI");
            CountryDictionary.Add("Sri Lanka", "LK");
            CountryDictionary.Add("Liberia", "LR");
            CountryDictionary.Add("Lesotho", "LS");
            CountryDictionary.Add("Lithuania", "LT");
            CountryDictionary.Add("Luxembourg", "LU");
            CountryDictionary.Add("Latvia", "LV");
            CountryDictionary.Add("Libya", "LY");
            CountryDictionary.Add("Morocco", "MA");
            CountryDictionary.Add("Monaco", "MC");
            CountryDictionary.Add("Moldova, Republic of", "MD");
            CountryDictionary.Add("Montenegro", "ME");
            CountryDictionary.Add("Saint Martin (French part)", "MF");
            CountryDictionary.Add("Madagascar", "MG");
            CountryDictionary.Add("Marshall Islands", "MH");
            CountryDictionary.Add("Macedonia, the former Yugoslav Republic of", "MK");
            CountryDictionary.Add("Mali", "ML");
            CountryDictionary.Add("Myanmar", "MM");
            CountryDictionary.Add("Mongolia", "MN");
            CountryDictionary.Add("Macao", "MO");
            CountryDictionary.Add("Northern Mariana Islands", "MP");
            CountryDictionary.Add("Martinique", "MQ");
            CountryDictionary.Add("Mauritania", "MR");
            CountryDictionary.Add("Montserrat", "MS");
            CountryDictionary.Add("Malta", "MT");
            CountryDictionary.Add("Mauritius", "MU");
            CountryDictionary.Add("Maldives", "MV");
            CountryDictionary.Add("Malawi", "MW");
            CountryDictionary.Add("Mexico", "MX");
            CountryDictionary.Add("Malaysia", "MY");
            CountryDictionary.Add("Mozambique", "MZ");
            CountryDictionary.Add("Namibia", "NA");
            CountryDictionary.Add("New Caledonia", "NC");
            CountryDictionary.Add("Niger", "NE");
            CountryDictionary.Add("Norfolk Island", "NF");
            CountryDictionary.Add("Nigeria", "NG");
            CountryDictionary.Add("Nicaragua", "NI");
            CountryDictionary.Add("Netherlands", "NL");
            CountryDictionary.Add("Norway", "NO");
            CountryDictionary.Add("Nepal", "NP");
            CountryDictionary.Add("Nauru", "NR");
            CountryDictionary.Add("Niue", "NU");
            CountryDictionary.Add("New Zealand", "NZ");
            CountryDictionary.Add("Oman", "OM");
            CountryDictionary.Add("Panama", "PA");
            CountryDictionary.Add("Peru", "PE");
            CountryDictionary.Add("French Polynesia", "PF");
            CountryDictionary.Add("Papua New Guinea", "PG");
            CountryDictionary.Add("Philippines", "PH");
            CountryDictionary.Add("Pakistan", "PK");
            CountryDictionary.Add("Poland", "PL");
            CountryDictionary.Add("Saint Pierre and Miquelon", "PM");
            CountryDictionary.Add("Pitcairn", "PN");
            CountryDictionary.Add("Puerto Rico", "PR");
            CountryDictionary.Add("Palestine, State of", "PS");
            CountryDictionary.Add("Portugal", "PT");
            CountryDictionary.Add("Palau", "PW");
            CountryDictionary.Add("Paraguay", "PY");
            CountryDictionary.Add("Qatar", "QA");
            CountryDictionary.Add("Réunion", "RE");
            CountryDictionary.Add("Romania", "RO");
            CountryDictionary.Add("Serbia", "RS");
            CountryDictionary.Add("Russian Federation", "RU");
            CountryDictionary.Add("Rwanda", "RW");
            CountryDictionary.Add("Saudi Arabia", "SA");
            CountryDictionary.Add("Solomon Islands", "SB");
            CountryDictionary.Add("Seychelles", "SC");
            CountryDictionary.Add("Sudan", "SD");
            CountryDictionary.Add("Sweden", "SE");
            CountryDictionary.Add("Singapore", "SG");
            CountryDictionary.Add("Saint Helena, Ascension and Tristan da Cunha", "SH");
            CountryDictionary.Add("Slovenia", "SI");
            CountryDictionary.Add("Svalbard and Jan Mayen", "SJ");
            CountryDictionary.Add("Slovakia", "SK");
            CountryDictionary.Add("Sierra Leone", "SL");
            CountryDictionary.Add("San Marino", "SM");
            CountryDictionary.Add("Senegal", "SN");
            CountryDictionary.Add("Somalia", "SO");
            CountryDictionary.Add("Suriname", "SR");
            CountryDictionary.Add("South Sudan", "SS");
            CountryDictionary.Add("Sao Tome and Principe", "ST");
            CountryDictionary.Add("El Salvador", "SV");
            CountryDictionary.Add("Sint Maarten (Dutch part)", "SX");
            CountryDictionary.Add("Syrian Arab Republic", "SY");
            CountryDictionary.Add("Swaziland", "SZ");
            CountryDictionary.Add("Turks and Caicos Islands", "TC");
            CountryDictionary.Add("Chad", "TD");
            CountryDictionary.Add("French Southern Territories", "TF");
            CountryDictionary.Add("Togo", "TG");
            CountryDictionary.Add("Thailand", "TH");
            CountryDictionary.Add("Tajikistan", "TJ");
            CountryDictionary.Add("Tokelau", "TK");
            CountryDictionary.Add("Timor-Leste", "TL");
            CountryDictionary.Add("Turkmenistan", "TM");
            CountryDictionary.Add("Tunisia", "TN");
            CountryDictionary.Add("Tonga", "TO");
            CountryDictionary.Add("Turkey", "TR");
            CountryDictionary.Add("Trinidad and Tobago", "TT");
            CountryDictionary.Add("Tuvalu", "TV");
            CountryDictionary.Add("Taiwan, Province of China", "TW");
            CountryDictionary.Add("Tanzania, United Republic of", "TZ");
            CountryDictionary.Add("Ukraine", "UA");
            CountryDictionary.Add("Uganda", "UG");
            CountryDictionary.Add("United States Minor Outlying Islands", "UM");
            CountryDictionary.Add("United States", "US");
            CountryDictionary.Add("Uruguay", "UY");
            CountryDictionary.Add("Uzbekistan", "UZ");
            CountryDictionary.Add("Holy See (Vatican City State)", "VA");
            CountryDictionary.Add("Saint Vincent and the Grenadines", "VC");
            CountryDictionary.Add("Venezuela, Bolivarian Republic of", "VE");
            CountryDictionary.Add("Virgin Islands, British", "VG");
            CountryDictionary.Add("Virgin Islands, U.S.", "VI");
            CountryDictionary.Add("Viet Nam", "VN");
            CountryDictionary.Add("Vanuatu", "VU");
            CountryDictionary.Add("Wallis and Futuna", "WF");
            CountryDictionary.Add("Samoa", "WS");
            CountryDictionary.Add("Yemen", "YE");
            CountryDictionary.Add("Mayotte", "YT");
            CountryDictionary.Add("South Africa", "ZA");
            CountryDictionary.Add("Zambia", "ZM");
            CountryDictionary.Add("Zimbabwe", "ZW");

            foreach (KeyValuePair<string, string> pair in CountryDictionary)
            {
                DeviceCountryComboBox.Items.Add(pair.Key);
            }

            DeviceCountryComboBox.SelectedIndex = -1;
            Log.Information($"DeviceCountryControl:InitializeCountryCodes Exit");
        }

        private void DeviceCountryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DeviceCountrySubmitButton.Enabled = (DeviceCountryComboBox.SelectedIndex != -1);
        }

        /// <summary>
        /// set the index to the provided country code
        /// </summary>
        /// <param name="countryCode"></param>
        public void SetCountryCode(string countryCode)
        {
            Log.Information($"DeviceCountryControl:SetCountryCode Entry countryCode :{countryCode}");
            //find the index of the country code - set the selected index to the index of the country code
            if (string.IsNullOrEmpty(countryCode) == false)
            {
                int selectedIndex = 0;
                foreach (KeyValuePair<string, string> pair in CountryDictionary)
                {
                    if (string.Equals(pair.Value, countryCode, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        DeviceCountryComboBox.SelectedIndex = selectedIndex;
                        break;
                    }

                    selectedIndex++;
                }
            }
            Log.Information($"DeviceCountryControl:SetCountryCode Exit countryCode :{countryCode}");
        }

        private void DeviceCountrySubmitButton_Click(object sender, EventArgs e)
        {
            //set the country code in the business layer
            AuthenticationService.Instance().SetDeviceCountryCode(CountryDictionary[(string)DeviceCountryComboBox.SelectedItem]);
            DeviceCountrySubmitDone?.Invoke(sender, e);

        }
    }
}
