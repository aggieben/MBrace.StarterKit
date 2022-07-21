// #I __SOURCE_DIRECTORY__
// #I "../packages/MBrace.Thespian/tools"
// #I "../packages/Streams/lib/netstandard2.0"
// #r "../packages/Streams/lib/netstandard2.0/Streams.dll"
// #I "../packages/MBrace.Flow/lib/net45"
// #r "../packages/MBrace.Flow/lib/net45/MBrace.Flow.dll"
// #load "../packages/MBrace.Thespian/MBrace.Thespian.fsx"
#r "paket: nuget MBrace.Flow ~> 1.6.0-alpha.2 prerelease"
#r "paket: nuget MBrace.Thespian ~> 1.6.0-alpha.2"

namespace global

module Config =

    open MBrace.Core
    open MBrace.Runtime
    open MBrace.Thespian

    // change to alter cluster size
    let private workerCount = 4

    let mutable private thespian = None

    do
        ThespianWorker.LocalExecutable <- (__SOURCE_DIRECTORY__ + "/../../../MBrace.Core/src/MBrace.Thespian.Worker/bin/Debug/net6/mbrace.thespian.worker.exe")

    /// Gets or creates a new Thespian cluster session.
    let GetCluster() =
        match thespian with
        | None ->
            let cluster =
                ThespianCluster.InitOnCurrentMachine(workerCount,
                                                     logger = new ConsoleLogger(),
                                                     logLevel = LogLevel.Info)
            thespian <- Some cluster
        | Some t -> ()
        thespian.Value

    /// Kills the current cluster session
    let KillCluster() =
        match thespian with
        | None -> ()
        | Some t -> t.KillAllWorkers() ; thespian <- None