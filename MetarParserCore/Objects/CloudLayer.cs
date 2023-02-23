using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using MetarParserCore.Enums;
using MetarParserCore.Extensions;

namespace MetarParserCore.Objects
{
    /// <summary>
    /// Info about clouds and vertical visibility (Cloud layers)
    /// </summary>
    [DataContract(Name = "cloudLayer")]
    public class CloudLayer
    {
        /// <summary>
        /// Cloud type
        /// </summary>
        [DataMember(Name = "cloudType", EmitDefaultValue = false)]
        public CloudType CloudType { get; init; }

        /// <summary>
        /// Cloud altitude
        /// </summary>
        [DataMember(Name = "altitude", EmitDefaultValue = false)]
        public int Altitude { get; init; }

        /// <summary>
        /// Convective cloud type
        /// </summary>
        [DataMember(Name = "convectiveCloudType", EmitDefaultValue = false)]
        public ConvectiveCloudType ConvectiveCloudType { get; init; }

        /// <summary>
        /// Cloud below airport (in mountain airports)
        /// </summary>
        [DataMember(Name = "isCloudBelow", EmitDefaultValue = false)]
        public bool IsCloudBelow { get; init; }

        #region Constructors

        /// <summary>
        /// Default
        /// </summary>
        public CloudLayer() { }

        /// <summary>
        /// Parser constructor
        /// </summary>
        /// <param name="tokens">Array of tokens</param>
        /// <param name="errors">List of parse errors</param>
        internal CloudLayer(string[] tokens, List<string> errors)
        {
            if (tokens.Length == 0)
            {
                errors.Add("Array of cloud layer tokens is empty");
                return;
            }

            var cloudToken = tokens.First();
            var parsedCloudType = ParseCloudType(cloudToken);
            cloudToken = parsedCloudType.Item2;

            CloudType = parsedCloudType.Item1;
            if (CloudType is CloudType.SkyClear
                or CloudType.Clear
                or CloudType.NoCloudDetected
                or CloudType.NoSignificantClouds)
                return;

            var parsedAltitude = GetAltitude(cloudToken, errors, out var isCloudBelow);
            Altitude = parsedAltitude.Item1;
            cloudToken = parsedAltitude.Item2;

            IsCloudBelow = isCloudBelow;
            ConvectiveCloudType = GetConvectiveCloudType(cloudToken);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Parse the current cloud type
        /// </summary>
        /// <param name="token">String token</param>
        /// <returns>Cloud type; new token value</returns>
        private (CloudType, string) ParseCloudType(string token)
        {
            if (token.StartsWith("VV"))
            {
                token = token.Replace("VV", "");
                return (CloudType.VerticalVisibility, token);
            }

            var cloudType = EnumTranslator.GetValueByDescription<CloudType>(token[..3]);
            return (cloudType, token[3..]);
        }

        /// <summary>
        /// Get FL altitude of the cloud
        /// </summary>
        /// <param name="token">String token</param>
        /// <param name="errors">Errors list</param>
        /// <param name="isCloudBelow">Sign if cloud is below airport</param>
        /// <returns>Altitude FL; new token value</returns>
        private (int, string) GetAltitude(string token, List<string> errors, out bool isCloudBelow)
        {
            isCloudBelow = false;

            if (token.StartsWith("///"))
            {
                isCloudBelow = true;
                return (0, token);
            }

            if (!int.TryParse(token[..3], out var altitude))
            {
                errors.Add($"Cannot parse altitude from token {token}");
                return (0, token);
            }

            return (altitude, token[3..]);
        }

        /// <summary>
        /// Get convective cloud type
        /// </summary>
        /// <param name="token">String token</param>
        /// <returns></returns>
        private ConvectiveCloudType GetConvectiveCloudType(string token)
        {
            return string.IsNullOrEmpty(token)
                ? ConvectiveCloudType.None
                : EnumTranslator.GetValueByDescription<ConvectiveCloudType>(token);
        }

        #endregion
    }
}