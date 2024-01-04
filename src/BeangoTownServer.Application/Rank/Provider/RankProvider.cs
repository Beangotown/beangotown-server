using System.Threading.Tasks;
using BeangoTownServer.Common;
using GraphQL;
using Volo.Abp.DependencyInjection;

namespace BeangoTownServer.Rank.Provider;


public class RankProvider : IRankProvider, ISingletonDependency
{
    private readonly IGraphQLHelper _graphQlHelper;

    public RankProvider(IGraphQLHelper graphQlHelper)
    {
        _graphQlHelper = graphQlHelper;
    }

    public async Task<WeekRankResultDto> GetWeekRankAsync(string caAddress, int skipCount, int maxResultCount)
    {
        var graphQLResponse = await _graphQlHelper.QueryAsync<WeekRankResultGraphDto>(new GraphQLRequest
        {
            Query = @"
			    query($skipCount:Int!,$maxResultCount:Int!,$caAddress:String!) {
                    getWeekRank(getRankDto:{skipCount: $skipCount,maxResultCount:$maxResultCount,caAddress:$caAddress})
                        {
                              status
                              refreshTime
                              rankingList{
                                rank
                                score
                              caAddress
                              }
                              selfRank{
                                rank
                                score
                                caAddress
                              }
                        }
                }",
            Variables = new
            {
                skipCount,
                maxResultCount,
                caAddress
            }
        });
        return graphQLResponse.GetWeekRank;
    }


    public async Task<SeasonRecordDto> GetSeasonConfigAsync()
    {
        var graphQLResponse = await _graphQlHelper.QueryAsync<SeasonRecordDto>(new GraphQLRequest
        {
            Query = @"
			    query{
                    getRankingSeasonList{
                      season{
                          id
                          name
                          playerWeekRankCount
                          playerWeekShowCount
                          playerSeasonRankCount
                          playerSeasonShowCount
                          rankBeginTime
                          rankEndTime
                          showBeginTime
                          showEndTime 
                                weekInfos{
                                    rankBeginTime
                                    rankEndTime
                                    showBeginTime
                                    showEndTime
                                }
                        }
                    }
                }"
        });
        return graphQLResponse;
    }
    
    public async Task<WeekRankRecordDto> GetWeekRankRecordsAsync(string seasonId, int week, int skipCount, int maxResultCount)
    {
        var graphQLResponse = await _graphQlHelper.QueryAsync<WeekRankRecordDto>(new GraphQLRequest
        {
            Query = @"
			    query($skipCount:Int!,$maxResultCount:Int!,$seasonId:String!,$week:Int!) {
                    getWeekRankRecords(getWeekRankDto:{skipCount:$skipCount,maxResultCount:$maxResultCount,seasonId:$seasonId,week:$week})
                      {
                        rankingList{
                          rank
                          score
                          caAddress
                        }
                      }
                    }",
            Variables = new
            {
                skipCount,
                maxResultCount,
                seasonId = seasonId,
                week = week
            }
        });
        return graphQLResponse;
    }

    public async Task<SeasonRankRecordDto> GetSeasonRankRecordsAsync(string seasonId, int skipCount, int maxResultCount)
    {
        var graphQLResponse = await _graphQlHelper.QueryAsync<SeasonRankRecordDto>(new GraphQLRequest
        {
            Query = @"
			    query($skipCount:Int!,$maxResultCount:Int!,$seasonId:String!) {
                    getSeasonRankRecords(getSeasonRankDto:{skipCount:$skipCount,maxResultCount:$maxResultCount,seasonId:$seasonId})
                      {
                        rankingList{
                           rank
                           score
                           caAddress
                        }
                      }
                    }",
            Variables = new
            {
                skipCount,
                maxResultCount,
                seasonId = seasonId
            }
        });
        return graphQLResponse;
    }

    public async Task<SeasonDto> GetSeasonAsync(string seasonId)
    {
        var graphQLResponse = await _graphQlHelper.QueryAsync<SeasonGraphDto>(new GraphQLRequest
        {
            Query = @"
			    query($seasonId:String!) {
                   getSeasonConfig(getSeasonDto:{
                       seasonId:$seasonId
                    }){
                        id
                        name
                        playerWeekRankCount
                        playerWeekShowCount
                        playerSeasonRankCount
                        playerSeasonShowCount
                        rankBeginTime
                        rankEndTime
                        showBeginTime
                        showEndTime 
                        weekInfos{
                            rankBeginTime
                            rankEndTime
                            showBeginTime
                            showEndTime
                        }
                    }
                    }",
            Variables = new
            {
                seasonId
            }
        });
        return graphQLResponse.GetSeasonConfig;
    }

    public async Task<GameBlockHeightDto> GetLatestGameByBlockHeightAsync(long blockHeight)
    {
        var graphQLResponse = await _graphQlHelper.QueryAsync<GameBlockHeightGraphDto>(new GraphQLRequest
        {
            Query = @"
			    query($blockHeight:Long!) {
                    getLatestGameByBlockHeight(getLatestGameHisDto:{
                      blockHeight: $blockHeight
                    }){
                      seasonId
                      latestGameId
                      bingoBlockHeight
                      gameCount
                      bingoTime
                     }
            }",
            Variables = new
            {
                blockHeight
            }
        });
        return graphQLResponse.GetLatestGameByBlockHeight;
    }

    public async Task<RankingHisResultDto> GetRankingHistoryAsync(GetRankingHisDto getRankingHisDto)
    {
        var graphQLResponse = await _graphQlHelper.QueryAsync<RankingHisResultGraphDto>(new GraphQLRequest
        {
            Query = @"
			    query($seasonId:String!,$caAddress:String!) {
                   getRankingHistory(getRankingHisDto:{seasonId:$seasonId ,caAddress:$caAddress}){
                   weeks{
                        week
                        caAddress
                        score
                        rank
                      }
               }
            }",
            Variables = new
            {
                getRankingHisDto.CaAddress, getRankingHisDto.SeasonId
            }
        });
        return graphQLResponse.GetRankingHistory;
    }
}