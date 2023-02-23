namespace MetarParserCore.TokenLogic
{
    /// <summary>
    /// Regular expressions for detect token type
    /// </summary>
    internal class TokenRegex
    {
        public static string Airport => @"^[A-Z]{4}$";

        public static string ObservationDayTime => @"^[0-3]{1}\d{1}[0-2]{1}\d{1}[0-5]{1}\d{1}Z$";

        public static string Modifier => @"^(COR|AUTO)$";

        public static string SurfaceWind => @"^(([0-3]{1}\d{2}|VRB)\d{2}(G\d{2})?(MPS|KMT|KT){1}|[0-3]{1}\d{2}V[0-3]{1}\d{2})$";

        public static string PrevailingVisibility => @"^(\d{4}(N|S)?(E|W)?|[1-2]{1}|M?(\d{1}\/\d{1,2}|\d{1,2})SM|CAVOK)$";

        public static string RunwayVisualRange => @"^R[0-3]{1}\d{1}(L|C|R|LL|RR)?\/(M|P)?\d{4}(V\d{4})?(FT)?(\/(U|D|N)|(U|D|N))?$";

        public static string PresentWeather => @"^(-|\+|VC)?(BC|BL|DR|FZ|MI|PR|SH|TS)?(DZ|GR|GS|IC|PL|RA|SG|SN|UP|BR|DU|FG|FU|HZ|PY|SA|VA|DS|FC|PO|SQ|SS){1,3}$";

        public static string CloudLayer => @"^((FEW|SCT|BKN|OVC|VV)\d{3}(CB|TCU)?|SKC|NSC|CLR|NCD)$";

        public static string Temperature => @"^M?\d{2}\/(M?\d{2})?$";

        public static string AltimeterSetting => @"^(A|Q){1}\d{4}$";

        public static string Motne => @"^((R[0-3]{1}\d{1}\/|\d{2})([0-9\/]{1}[1259\/]{1}\d{4}|(CLRD|CLSD)\d{2})|R/SNOCLO|SNOCLO)$";

        public static string SeaState => @"^W(M)?\d{2}\/(S[0-9]{1}|H\d{1,2})$";

        public static string TrendTime => @"^(AT|FM|TL)[0-2]{1}\d{1}[0-5]{1}\d{1}$";

        public static string MilitaryCode => @"^(BLACK)?(BLU|WHT|GRN|YLO|AMB|RED)$";
    }
}
