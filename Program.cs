using System.Collections.Generic;
using Pulumi;
using Pulumi.Gcp.Compute;
using Pulumi.Gcp.Compute.Inputs;

return await Deployment.RunAsync(() =>
{
    var instance = new Instance("my-instance", new InstanceArgs
    {
        Zone = "us-west1-a",
        MachineType = "e2-micro",
        Scheduling = new InstanceSchedulingArgs
        {
            Preemptible = false,
        },
        BootDisk = new InstanceBootDiskArgs
        {
            InitializeParams = new InstanceBootDiskInitializeParamsArgs
            {
                Size = 30,
                Type = "pd-standard",
                Image = "debian-cloud/debian-12",
            },
        },
        NetworkInterfaces = new[]
        {
            new InstanceNetworkInterfaceArgs
            {
                Network = "default",
                AccessConfigs = new[]
                {
                    new InstanceNetworkInterfaceAccessConfigArgs
                    {
                        NetworkTier = "STANDARD",
                    },
                },
            },
        },
        Tags = ["http-server", "https-server"],
    });

    return new Dictionary<string, object?>
    {
        ["NatIp"] = instance.NetworkInterfaces.Apply(x => x[0].AccessConfigs[0].NatIp),
    };
});