using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using MetarParserCore.Enums;

namespace MetarParserCore.Objects
{
    /// <summary>
    /// Info about runway conditions
    /// </summary>
    [DataContract(Name = "motne")]
    public class Motne
    {
        /// <summary>
        /// Current runway
        /// </summary>
        [DataMember(Name = "runwayNumber", EmitDefaultValue = false)]
        public string RunwayNumber { get; init; }

        /// <summary>
        /// MOTNE special sign
        /// </summary>
        [DataMember(Name = "specials", EmitDefaultValue = false)]
        public MotneSpecials Specials { get; init; }

        /// <summary>
        /// Type of deposit
        /// </summary>
        [DataMember(Name = "typeOfDeposit")]
        public MotneTypeOfDeposit TypeOfDeposit { get; init; }

        /// <summary>
        /// Extent of contamination of the current runway
        /// </summary>
        [DataMember(Name = "extentOfContamination", EmitDefaultValue = false)]
        public MotneExtentOfContamination ExtentOfContamination { get; init; }

        /// <summary>
        /// Depth of deposit (2 digits)
        /// -1 - depth not significant (has value "//")
        /// </summary>
        [DataMember(Name = "depthOfDeposit", EmitDefaultValue = false)]
        public int DepthOfDeposit { get; init; }

        /// <summary>
        /// Braking conditions
        /// -1 - not measured (has value "//")
        /// </summary>
        [DataMember(Name = "frictionCoefficient", EmitDefaultValue = false)]
        public int FrictionCoefficient { get; init; }

        /// <summary>
        /// Default
        /// </summary>
        public Motne() { }

        /// <summary>
        /// Parse constructor
        /// </summary>
        internal Motne(string[] tokens, List<string> errors)
        {
            if (tokens.Length == 0)
            {
                errors.Add("Motne token is not found in incoming array");
                return;
            }

            var motneToken = tokens.First();
            if (string.IsNullOrEmpty(motneToken))
            {
                errors.Add("Motne token was not found");
                return;
            }

            switch (motneToken)
            {
                case { } when motneToken.Contains("CLRD"):
                    FrictionCoefficient = GetMotneIntValue(motneToken, motneToken.Length - 2, 2);
                    RunwayNumber = GetRunwayNumber(motneToken, errors).Item1;
                    Specials = MotneSpecials.Cleared;
                    return;
                case { } when motneToken.Contains("CLSD"):
                    FrictionCoefficient = GetMotneIntValue(motneToken, motneToken.Length - 2, 2);
                    RunwayNumber = GetRunwayNumber(motneToken, errors).Item1;
                    Specials = MotneSpecials.Closed;
                    return;
                case { } when motneToken.Contains("SNOCLO"):
                    Specials = MotneSpecials.ClosedToSnow;
                    return;
            }

            var parsedRunwayNumber = GetRunwayNumber(motneToken, errors);
            RunwayNumber = parsedRunwayNumber.Item1;
            motneToken = parsedRunwayNumber.Item2;

            TypeOfDeposit = motneToken.Substring(0, 1).Equals("/")
                ? MotneTypeOfDeposit.NotReported
                : GetMotneEnum<MotneTypeOfDeposit>(motneToken.Substring(0, 1));
            ExtentOfContamination = GetMotneEnum<MotneExtentOfContamination>(motneToken.Substring(1, 1));
            DepthOfDeposit = GetMotneIntValue(motneToken, 2, 2);
            FrictionCoefficient = GetMotneIntValue(motneToken, motneToken.Length - 2, 2);
            Specials = MotneSpecials.Default;
        }

        /// <summary>
        /// Parse runway number
        /// <param name="motneToken">Current MOTNE</param>
        /// <param name="errors">Errors list</param>
        /// </summary>
        /// <returns>Runway number; new MOTNE token value</returns>
        private (string, string) GetRunwayNumber(string motneToken, List<string> errors)
        {
            if (motneToken.StartsWith("R"))
            {
                var splittedMotne = motneToken.Split("/");
                return (splittedMotne[0][1..], motneToken[(splittedMotne[0].Length + 1)..]);
            }

            var stringNumber = motneToken[..2];
            var runwayNumber = int.Parse(stringNumber);
            if (runwayNumber > 86 && runwayNumber != 88 && runwayNumber != 99)
            {
                errors.Add($"Incorrect runway number in MOTNE {motneToken} token");
                return (string.Empty, motneToken);
            }

            return (stringNumber, motneToken[2..]);
        }

        /// <summary>
        /// Get MOTNE data as enum value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stringValue">MOTNE string value</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private T GetMotneEnum<T>(string stringValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            if (stringValue.Contains("/"))
                return default;

            return (T)(object)int.Parse(stringValue);
        }

        /// <summary>
        /// Get MOTNE data as integer value
        /// </summary>
        /// <param name="motneToken">Current MOTNE token</param>
        /// <param name="startIdx">Substring start index</param>
        /// <param name="length">Elements length</param>
        /// <returns></returns>
        private int GetMotneIntValue(string motneToken, int startIdx, int length)
        {
            var valueToken = motneToken.Substring(startIdx, length);
            if (valueToken.Contains("/"))
                return -1;

            return int.Parse(valueToken);
        }
    }
}
