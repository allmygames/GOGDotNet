namespace GOGDotNet.Models
{
    public enum ProfileState
    {
        /// <summary>
        /// GOG profile exists and is accessible.
        /// </summary>
        Verified,

        /// <summary>
        /// GOG profile does not exist.
        /// </summary>
        DoesNotExist,

        /// <summary>
        /// GOG profile is private and cannot be read.
        /// </summary>
        Private,

        /// <summary>
        /// Existence of GOG profile could not be verified.
        /// </summary>
        VerificationFailed,
    }
}
