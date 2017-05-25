using NUnit.Framework;
using PullString;

public class Version
{
    [Test]
    public void Test_VersionInfo()
    {
        string correctApiBase = "https://conversation.pullstring.ai/v1/";
        Assert.IsTrue(VersionInfo.ApiBaseUrl.Equals(correctApiBase), "Unexpected Base Url. It should be: " + correctApiBase);
        Assert.IsTrue(VersionInfo.HasFeature(EFeatureName.StreamingAsr), "ASR should be enabled");
    }
}