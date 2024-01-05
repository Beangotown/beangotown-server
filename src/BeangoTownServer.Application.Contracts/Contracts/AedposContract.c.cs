// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: aedpos_contract.proto
// </auto-generated>
// Original file comments:
// *
// AEDPoS contract.
#pragma warning disable 0414, 1591
#region Designer generated code

using System.Collections.Generic;
using aelf = global::AElf.CSharp.Core;

namespace AElf.Contracts.Consensus.AEDPoS {

  #region Events
  internal partial class IrreversibleBlockFound : aelf::IEvent<IrreversibleBlockFound>
  {
    public global::System.Collections.Generic.IEnumerable<IrreversibleBlockFound> GetIndexed()
    {
      return new List<IrreversibleBlockFound>
      {
      new IrreversibleBlockFound
      {
        IrreversibleBlockHeight = IrreversibleBlockHeight
      },
      };
    }

    public IrreversibleBlockFound GetNonIndexed()
    {
      return new IrreversibleBlockFound
      {
      };
    }
  }

  internal partial class IrreversibleBlockHeightUnacceptable : aelf::IEvent<IrreversibleBlockHeightUnacceptable>
  {
    public global::System.Collections.Generic.IEnumerable<IrreversibleBlockHeightUnacceptable> GetIndexed()
    {
      return new List<IrreversibleBlockHeightUnacceptable>
      {
      };
    }

    public IrreversibleBlockHeightUnacceptable GetNonIndexed()
    {
      return new IrreversibleBlockHeightUnacceptable
      {
        DistanceToIrreversibleBlockHeight = DistanceToIrreversibleBlockHeight,
      };
    }
  }

  internal partial class MiningInformationUpdated : aelf::IEvent<MiningInformationUpdated>
  {
    public global::System.Collections.Generic.IEnumerable<MiningInformationUpdated> GetIndexed()
    {
      return new List<MiningInformationUpdated>
      {
      new MiningInformationUpdated
      {
        Pubkey = Pubkey
      },
      new MiningInformationUpdated
      {
        MiningTime = MiningTime
      },
      new MiningInformationUpdated
      {
        Behaviour = Behaviour
      },
      new MiningInformationUpdated
      {
        BlockHeight = BlockHeight
      },
      new MiningInformationUpdated
      {
        PreviousBlockHash = PreviousBlockHash
      },
      };
    }

    public MiningInformationUpdated GetNonIndexed()
    {
      return new MiningInformationUpdated
      {
      };
    }
  }

  internal partial class SecretSharingInformation : aelf::IEvent<SecretSharingInformation>
  {
    public global::System.Collections.Generic.IEnumerable<SecretSharingInformation> GetIndexed()
    {
      return new List<SecretSharingInformation>
      {
      new SecretSharingInformation
      {
        PreviousRound = PreviousRound
      },
      };
    }

    public SecretSharingInformation GetNonIndexed()
    {
      return new SecretSharingInformation
      {
        CurrentRoundId = CurrentRoundId,
        PreviousRoundId = PreviousRoundId,
      };
    }
  }

  internal partial class MiningRewardGenerated : aelf::IEvent<MiningRewardGenerated>
  {
    public global::System.Collections.Generic.IEnumerable<MiningRewardGenerated> GetIndexed()
    {
      return new List<MiningRewardGenerated>
      {
      new MiningRewardGenerated
      {
        TermNumber = TermNumber
      },
      };
    }

    public MiningRewardGenerated GetNonIndexed()
    {
      return new MiningRewardGenerated
      {
        Amount = Amount,
      };
    }
  }

  internal partial class MinerReplaced : aelf::IEvent<MinerReplaced>
  {
    public global::System.Collections.Generic.IEnumerable<MinerReplaced> GetIndexed()
    {
      return new List<MinerReplaced>
      {
      };
    }

    public MinerReplaced GetNonIndexed()
    {
      return new MinerReplaced
      {
        NewMinerPubkey = NewMinerPubkey,
      };
    }
  }

  #endregion
  internal static partial class AEDPoSContractContainer
  {
    static readonly string __ServiceName = "AEDPoS.AEDPoSContract";

    #region Marshallers
    static readonly aelf::Marshaller<global::AElf.Contracts.Consensus.AEDPoS.InitialAElfConsensusContractInput> __Marshaller_AEDPoS_InitialAElfConsensusContractInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::AElf.Contracts.Consensus.AEDPoS.InitialAElfConsensusContractInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Google.Protobuf.WellKnownTypes.Empty> __Marshaller_google_protobuf_Empty = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Protobuf.WellKnownTypes.Empty.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::AElf.Contracts.Consensus.AEDPoS.Round> __Marshaller_AEDPoS_Round = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::AElf.Contracts.Consensus.AEDPoS.Round.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::AElf.Contracts.Consensus.AEDPoS.UpdateValueInput> __Marshaller_AEDPoS_UpdateValueInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::AElf.Contracts.Consensus.AEDPoS.UpdateValueInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::AElf.Contracts.Consensus.AEDPoS.NextRoundInput> __Marshaller_AEDPoS_NextRoundInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::AElf.Contracts.Consensus.AEDPoS.NextRoundInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::AElf.Contracts.Consensus.AEDPoS.NextTermInput> __Marshaller_AEDPoS_NextTermInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::AElf.Contracts.Consensus.AEDPoS.NextTermInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::AElf.Contracts.Consensus.AEDPoS.TinyBlockInput> __Marshaller_AEDPoS_TinyBlockInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::AElf.Contracts.Consensus.AEDPoS.TinyBlockInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Google.Protobuf.WellKnownTypes.Int32Value> __Marshaller_google_protobuf_Int32Value = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Protobuf.WellKnownTypes.Int32Value.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::AuthorityInfo> __Marshaller_AuthorityInfo = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::AuthorityInfo.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Google.Protobuf.WellKnownTypes.Int64Value> __Marshaller_google_protobuf_Int64Value = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Protobuf.WellKnownTypes.Int64Value.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::AElf.Contracts.Consensus.AEDPoS.RecordCandidateReplacementInput> __Marshaller_AEDPoS_RecordCandidateReplacementInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::AElf.Contracts.Consensus.AEDPoS.RecordCandidateReplacementInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::AElf.Contracts.Consensus.AEDPoS.MinerList> __Marshaller_AEDPoS_MinerList = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::AElf.Contracts.Consensus.AEDPoS.MinerList.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::AElf.Contracts.Consensus.AEDPoS.PubkeyList> __Marshaller_AEDPoS_PubkeyList = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::AElf.Contracts.Consensus.AEDPoS.PubkeyList.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::AElf.Contracts.Consensus.AEDPoS.MinerListWithRoundNumber> __Marshaller_AEDPoS_MinerListWithRoundNumber = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::AElf.Contracts.Consensus.AEDPoS.MinerListWithRoundNumber.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::AElf.Contracts.Consensus.AEDPoS.GetMinerListInput> __Marshaller_AEDPoS_GetMinerListInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::AElf.Contracts.Consensus.AEDPoS.GetMinerListInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Google.Protobuf.WellKnownTypes.StringValue> __Marshaller_google_protobuf_StringValue = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Protobuf.WellKnownTypes.StringValue.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::AElf.Types.Address> __Marshaller_aelf_Address = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::AElf.Types.Address.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Google.Protobuf.WellKnownTypes.BoolValue> __Marshaller_google_protobuf_BoolValue = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Protobuf.WellKnownTypes.BoolValue.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::AElf.Types.Hash> __Marshaller_aelf_Hash = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::AElf.Types.Hash.Parser.ParseFrom);
    #endregion

    #region Methods
    static readonly aelf::Method<global::AElf.Contracts.Consensus.AEDPoS.InitialAElfConsensusContractInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_InitialAElfConsensusContract = new aelf::Method<global::AElf.Contracts.Consensus.AEDPoS.InitialAElfConsensusContractInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "InitialAElfConsensusContract",
        __Marshaller_AEDPoS_InitialAElfConsensusContractInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::AElf.Contracts.Consensus.AEDPoS.Round, global::Google.Protobuf.WellKnownTypes.Empty> __Method_FirstRound = new aelf::Method<global::AElf.Contracts.Consensus.AEDPoS.Round, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "FirstRound",
        __Marshaller_AEDPoS_Round,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::AElf.Contracts.Consensus.AEDPoS.UpdateValueInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_UpdateValue = new aelf::Method<global::AElf.Contracts.Consensus.AEDPoS.UpdateValueInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "UpdateValue",
        __Marshaller_AEDPoS_UpdateValueInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::AElf.Contracts.Consensus.AEDPoS.NextRoundInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_NextRound = new aelf::Method<global::AElf.Contracts.Consensus.AEDPoS.NextRoundInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "NextRound",
        __Marshaller_AEDPoS_NextRoundInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::AElf.Contracts.Consensus.AEDPoS.NextTermInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_NextTerm = new aelf::Method<global::AElf.Contracts.Consensus.AEDPoS.NextTermInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "NextTerm",
        __Marshaller_AEDPoS_NextTermInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::AElf.Contracts.Consensus.AEDPoS.TinyBlockInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_UpdateTinyBlockInformation = new aelf::Method<global::AElf.Contracts.Consensus.AEDPoS.TinyBlockInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "UpdateTinyBlockInformation",
        __Marshaller_AEDPoS_TinyBlockInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Int32Value, global::Google.Protobuf.WellKnownTypes.Empty> __Method_SetMaximumMinersCount = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Int32Value, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "SetMaximumMinersCount",
        __Marshaller_google_protobuf_Int32Value,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::AuthorityInfo, global::Google.Protobuf.WellKnownTypes.Empty> __Method_ChangeMaximumMinersCountController = new aelf::Method<global::AuthorityInfo, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "ChangeMaximumMinersCountController",
        __Marshaller_AuthorityInfo,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Int64Value, global::Google.Protobuf.WellKnownTypes.Empty> __Method_SetMinerIncreaseInterval = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Int64Value, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "SetMinerIncreaseInterval",
        __Marshaller_google_protobuf_Int64Value,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::AElf.Contracts.Consensus.AEDPoS.RecordCandidateReplacementInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_RecordCandidateReplacement = new aelf::Method<global::AElf.Contracts.Consensus.AEDPoS.RecordCandidateReplacementInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "RecordCandidateReplacement",
        __Marshaller_AEDPoS_RecordCandidateReplacementInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.MinerList> __Method_GetCurrentMinerList = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.MinerList>(
        aelf::MethodType.View,
        __ServiceName,
        "GetCurrentMinerList",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_AEDPoS_MinerList);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.PubkeyList> __Method_GetCurrentMinerPubkeyList = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.PubkeyList>(
        aelf::MethodType.View,
        __ServiceName,
        "GetCurrentMinerPubkeyList",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_AEDPoS_PubkeyList);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.MinerListWithRoundNumber> __Method_GetCurrentMinerListWithRoundNumber = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.MinerListWithRoundNumber>(
        aelf::MethodType.View,
        __ServiceName,
        "GetCurrentMinerListWithRoundNumber",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_AEDPoS_MinerListWithRoundNumber);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Int64Value, global::AElf.Contracts.Consensus.AEDPoS.Round> __Method_GetRoundInformation = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Int64Value, global::AElf.Contracts.Consensus.AEDPoS.Round>(
        aelf::MethodType.View,
        __ServiceName,
        "GetRoundInformation",
        __Marshaller_google_protobuf_Int64Value,
        __Marshaller_AEDPoS_Round);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value> __Method_GetCurrentRoundNumber = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value>(
        aelf::MethodType.View,
        __ServiceName,
        "GetCurrentRoundNumber",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_google_protobuf_Int64Value);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.Round> __Method_GetCurrentRoundInformation = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.Round>(
        aelf::MethodType.View,
        __ServiceName,
        "GetCurrentRoundInformation",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_AEDPoS_Round);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.Round> __Method_GetPreviousRoundInformation = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.Round>(
        aelf::MethodType.View,
        __ServiceName,
        "GetPreviousRoundInformation",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_AEDPoS_Round);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value> __Method_GetCurrentTermNumber = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value>(
        aelf::MethodType.View,
        __ServiceName,
        "GetCurrentTermNumber",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_google_protobuf_Int64Value);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value> __Method_GetCurrentTermMiningReward = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value>(
        aelf::MethodType.View,
        __ServiceName,
        "GetCurrentTermMiningReward",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_google_protobuf_Int64Value);

    static readonly aelf::Method<global::AElf.Contracts.Consensus.AEDPoS.GetMinerListInput, global::AElf.Contracts.Consensus.AEDPoS.MinerList> __Method_GetMinerList = new aelf::Method<global::AElf.Contracts.Consensus.AEDPoS.GetMinerListInput, global::AElf.Contracts.Consensus.AEDPoS.MinerList>(
        aelf::MethodType.View,
        __ServiceName,
        "GetMinerList",
        __Marshaller_AEDPoS_GetMinerListInput,
        __Marshaller_AEDPoS_MinerList);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.MinerList> __Method_GetPreviousMinerList = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.MinerList>(
        aelf::MethodType.View,
        __ServiceName,
        "GetPreviousMinerList",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_AEDPoS_MinerList);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value> __Method_GetMinedBlocksOfPreviousTerm = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value>(
        aelf::MethodType.View,
        __ServiceName,
        "GetMinedBlocksOfPreviousTerm",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_google_protobuf_Int64Value);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.StringValue> __Method_GetNextMinerPubkey = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.StringValue>(
        aelf::MethodType.View,
        __ServiceName,
        "GetNextMinerPubkey",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_google_protobuf_StringValue);

    static readonly aelf::Method<global::AElf.Types.Address, global::Google.Protobuf.WellKnownTypes.BoolValue> __Method_IsCurrentMiner = new aelf::Method<global::AElf.Types.Address, global::Google.Protobuf.WellKnownTypes.BoolValue>(
        aelf::MethodType.View,
        __ServiceName,
        "IsCurrentMiner",
        __Marshaller_aelf_Address,
        __Marshaller_google_protobuf_BoolValue);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value> __Method_GetNextElectCountDown = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value>(
        aelf::MethodType.View,
        __ServiceName,
        "GetNextElectCountDown",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_google_protobuf_Int64Value);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Int64Value, global::AElf.Contracts.Consensus.AEDPoS.Round> __Method_GetPreviousTermInformation = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Int64Value, global::AElf.Contracts.Consensus.AEDPoS.Round>(
        aelf::MethodType.View,
        __ServiceName,
        "GetPreviousTermInformation",
        __Marshaller_google_protobuf_Int64Value,
        __Marshaller_AEDPoS_Round);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Int64Value, global::AElf.Types.Hash> __Method_GetRandomHash = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Int64Value, global::AElf.Types.Hash>(
        aelf::MethodType.View,
        __ServiceName,
        "GetRandomHash",
        __Marshaller_google_protobuf_Int64Value,
        __Marshaller_aelf_Hash);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int32Value> __Method_GetMaximumBlocksCount = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int32Value>(
        aelf::MethodType.View,
        __ServiceName,
        "GetMaximumBlocksCount",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_google_protobuf_Int32Value);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int32Value> __Method_GetMaximumMinersCount = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int32Value>(
        aelf::MethodType.View,
        __ServiceName,
        "GetMaximumMinersCount",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_google_protobuf_Int32Value);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AuthorityInfo> __Method_GetMaximumMinersCountController = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AuthorityInfo>(
        aelf::MethodType.View,
        __ServiceName,
        "GetMaximumMinersCountController",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_AuthorityInfo);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value> __Method_GetMinerIncreaseInterval = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value>(
        aelf::MethodType.View,
        __ServiceName,
        "GetMinerIncreaseInterval",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_google_protobuf_Int64Value);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.MinerList> __Method_GetMainChainCurrentMinerList = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.MinerList>(
        aelf::MethodType.View,
        __ServiceName,
        "GetMainChainCurrentMinerList",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_AEDPoS_MinerList);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.PubkeyList> __Method_GetPreviousTermMinerPubkeyList = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.PubkeyList>(
        aelf::MethodType.View,
        __ServiceName,
        "GetPreviousTermMinerPubkeyList",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_AEDPoS_PubkeyList);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value> __Method_GetCurrentMiningRewardPerBlock = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value>(
        aelf::MethodType.View,
        __ServiceName,
        "GetCurrentMiningRewardPerBlock",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_google_protobuf_Int64Value);

    #endregion

    #region Descriptors
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::AElf.Contracts.Consensus.AEDPoS.AedposContractReflection.Descriptor.Services[0]; }
    }

    public static global::System.Collections.Generic.IReadOnlyList<global::Google.Protobuf.Reflection.ServiceDescriptor> Descriptors
    {
      get
      {
        return new global::System.Collections.Generic.List<global::Google.Protobuf.Reflection.ServiceDescriptor>()
        {
          global::AElf.Contracts.Consensus.AEDPoS.AedposContractReflection.Descriptor.Services[0],
        };
      }
    }
    #endregion

    public class AEDPoSContractReferenceState : global::AElf.Sdk.CSharp.State.ContractReferenceState
    {
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::AElf.Contracts.Consensus.AEDPoS.InitialAElfConsensusContractInput, global::Google.Protobuf.WellKnownTypes.Empty> InitialAElfConsensusContract { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::AElf.Contracts.Consensus.AEDPoS.Round, global::Google.Protobuf.WellKnownTypes.Empty> FirstRound { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::AElf.Contracts.Consensus.AEDPoS.UpdateValueInput, global::Google.Protobuf.WellKnownTypes.Empty> UpdateValue { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::AElf.Contracts.Consensus.AEDPoS.NextRoundInput, global::Google.Protobuf.WellKnownTypes.Empty> NextRound { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::AElf.Contracts.Consensus.AEDPoS.NextTermInput, global::Google.Protobuf.WellKnownTypes.Empty> NextTerm { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::AElf.Contracts.Consensus.AEDPoS.TinyBlockInput, global::Google.Protobuf.WellKnownTypes.Empty> UpdateTinyBlockInformation { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Int32Value, global::Google.Protobuf.WellKnownTypes.Empty> SetMaximumMinersCount { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::AuthorityInfo, global::Google.Protobuf.WellKnownTypes.Empty> ChangeMaximumMinersCountController { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Int64Value, global::Google.Protobuf.WellKnownTypes.Empty> SetMinerIncreaseInterval { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::AElf.Contracts.Consensus.AEDPoS.RecordCandidateReplacementInput, global::Google.Protobuf.WellKnownTypes.Empty> RecordCandidateReplacement { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.MinerList> GetCurrentMinerList { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.PubkeyList> GetCurrentMinerPubkeyList { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.MinerListWithRoundNumber> GetCurrentMinerListWithRoundNumber { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Int64Value, global::AElf.Contracts.Consensus.AEDPoS.Round> GetRoundInformation { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value> GetCurrentRoundNumber { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.Round> GetCurrentRoundInformation { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.Round> GetPreviousRoundInformation { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value> GetCurrentTermNumber { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value> GetCurrentTermMiningReward { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::AElf.Contracts.Consensus.AEDPoS.GetMinerListInput, global::AElf.Contracts.Consensus.AEDPoS.MinerList> GetMinerList { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.MinerList> GetPreviousMinerList { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value> GetMinedBlocksOfPreviousTerm { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.StringValue> GetNextMinerPubkey { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::AElf.Types.Address, global::Google.Protobuf.WellKnownTypes.BoolValue> IsCurrentMiner { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value> GetNextElectCountDown { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Int64Value, global::AElf.Contracts.Consensus.AEDPoS.Round> GetPreviousTermInformation { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Int64Value, global::AElf.Types.Hash> GetRandomHash { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int32Value> GetMaximumBlocksCount { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int32Value> GetMaximumMinersCount { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::AuthorityInfo> GetMaximumMinersCountController { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value> GetMinerIncreaseInterval { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.MinerList> GetMainChainCurrentMinerList { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Contracts.Consensus.AEDPoS.PubkeyList> GetPreviousTermMinerPubkeyList { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int64Value> GetCurrentMiningRewardPerBlock { get; set; }
    }
  }
}
#endregion

