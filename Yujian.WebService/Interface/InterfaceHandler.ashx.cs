using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YuJian.WeiXin.BLL;
using YuJian.WeiXin.Entity;
using YuJian.WeiXin.Entity.Interface.Response;
using Yunchee.Volkswagen.Utility;
using Yunchee.Volkswagen.Utility.Log;
using Yunchee.Volkswagen.Utility.ExtensionMethod;
using YuJian.WeiXin.Entity.Interface.Request;
using Yujian.Entity.Interface.Response;
using Yujian.Entity.Interface.Request;
using System.Configuration;
using Yunchee.Volkswagen.Utility.DataAccess.Query;


namespace Yujian.WebService.Interface
{
    /// <summary>
    /// InterfaceHandler 的摘要说明
    /// </summary>
    public class InterfaceHandler
    {
        int _random = ConfigurationManager.AppSettings["Random"].ToInt();
        BasicUserInfo loginInfo = new BasicUserInfo();
        public string GetCurrentData()
        {
            string content = string.Empty;

            var response = new GetCurrentDataResponse();
            try
            {
                string reqContent = HttpContext.Current.Request["ReqContent"];

                if (!string.IsNullOrEmpty(reqContent))
                {
                    var request = reqContent.DeserializeJSONTo<GetCurrentDataRequest>();

                    Loggers.Debug(new DebugLogInfo()
                    {
                        Message = string.Format("GetCurrentData: {0}", reqContent)
                    });

                    response.Data = new GetCurrentDataData();
                    response.Data.LotteryNumber = 0;
                    response.Data.PrizeInfoList = new List<PrizeInfoEntity>();

                    var bll = new QualificationBLL(new BasicUserInfo());
                    var firstLogin = bll.QueryByEntity(new QualificationEntity() { Type = 1, WxOpenID = request.OpenID }, null);
                    if (firstLogin.Length == 0)
                    {
                        var entity = new QualificationEntity();
                        entity.WxID = "gh_0846d6c7c2b8";
                        entity.WxOpenID = request.OpenID;
                        entity.Type = 1;
                        entity.EnableFlag = 1;
                        bll.Create(entity);
                    }
                    var first = bll.QueryByEntity(new QualificationEntity() { Type = 1, EnableFlag = 1, WxOpenID = request.OpenID }, null);
                    if (first.Length > 0)
                        response.Data.LotteryNumber += 1;
                    var daily = bll.QueryEnableQualificationByCurrentTime(request.OpenID);
                    if (daily.Tables[0].Rows.Count > 0)
                        response.Data.LotteryNumber += 1;

                    int mapping1 = 0, mapping2 = 0, mapping3 = 0;
                    var drawAll = new DrawAllBLL(loginInfo).QueryByEntity(new DrawAllEntity { Openid = request.OpenID }, null);
                    if (drawAll.Length > 0)
                    {
                        mapping1 = drawAll.FirstOrDefault().Mapping1ID.Value;
                        mapping2 = drawAll.FirstOrDefault().Mapping2ID.Value;
                        mapping3 = drawAll.FirstOrDefault().Mapping3ID.Value;
                    }

                    var arr = new CustomerPrizeMappingBLL(new BasicUserInfo())
                                    .QueryByEntity(new CustomerPrizeMappingEntity() { Openid = request.OpenID, Enable = 1 }
                                        , new OrderBy[] { new OrderBy() { FieldName = "CreateTime", Direction = OrderByDirections.Asc } });
                    IEnumerable<CustomerPrizeMappingEntity> prizedInfoList;
                    if (drawAll.Length > 0 && arr.Length > 0)
                        prizedInfoList = arr.Where(s => s.ID != mapping1 && s.ID != mapping2 && s.ID != mapping3).ToArray();
                    else
                        prizedInfoList = arr;

                    foreach (var tmp in prizedInfoList)
                    {
                        response.Data.PrizeInfoList.Add(new PrizeInfoEntity() { PrizeID = tmp.PrizeID.Value });
                    }
                }
            }
            catch (Exception ex)
            {
                response.Code = "103";
                response.Description = "数据库操作错误";

                Loggers.Exception(new ExceptionLogInfo(ex));
            }

            content = response.ToJSON();
            return content;
        }

        public string PreDrawLottery()
        {
            string content = string.Empty;

            var response = new PreDrawLotteryResponse();
            try
            {
                string reqContent = HttpContext.Current.Request["ReqContent"];

                if (!string.IsNullOrEmpty(reqContent))
                {
                    var request = reqContent.DeserializeJSONTo<PreDrawLotteryRequest>();

                    Loggers.Debug(new DebugLogInfo()
                    {
                        Message = string.Format("PreDrawLottery: {0}", reqContent)
                    });

                    response.Data = new PreDrawLotteryData();
                    response.Data.PreDrawLotteryResult = 0;
                    response.Data.PrizeID = 0;
                    response.Data.MappingID = 0;
                    var bll = new QualificationBLL(new BasicUserInfo());
                    var firstLogin = bll.QueryByEntity(new QualificationEntity() { Type = 1, EnableFlag = 1, WxOpenID = request.OpenID }, null);
                    if (firstLogin.Length == 1)//第一次抽奖
                    {
                        int random = new Random().Next(1, _random);
                        if (random == 1)
                        {
                            int randomPrize = new Random().Next(1, 3);
                            var mapping = new CustomerPrizeMappingEntity();
                            mapping.Openid = request.OpenID;
                            mapping.PrizeID = randomPrize;
                            mapping.Enable = 0;
                            new CustomerPrizeMappingBLL(new BasicUserInfo()).Create(mapping);

                            response.Data.PreDrawLotteryResult = 1;
                            response.Data.PrizeID = randomPrize;
                            response.Data.MappingID = mapping.ID.Value;
                        }
                    }
                    else//第N次抽奖
                    {
                        var dailyLogin = bll.QueryEnableQualificationByCurrentTime(request.OpenID);
                        if (dailyLogin.Tables[0].Rows.Count > 0)
                        {
                            var entity = DataTableToObject.ConvertToObject<QualificationEntity>(dailyLogin.Tables[0].Rows[0]);

                            int mapping1 = 0, mapping2 = 0, mapping3 = 0;
                            var drawAll = new DrawAllBLL(loginInfo).QueryByEntity(new DrawAllEntity { Openid = request.OpenID }, null);
                            if (drawAll.Length > 0)
                            {
                                mapping1 = drawAll.FirstOrDefault().Mapping1ID.Value;
                                mapping2 = drawAll.FirstOrDefault().Mapping2ID.Value;
                                mapping3 = drawAll.FirstOrDefault().Mapping3ID.Value;
                            }

                            var arr = new CustomerPrizeMappingBLL(new BasicUserInfo())
                                            .QueryByEntity(new CustomerPrizeMappingEntity() { Openid = request.OpenID, Enable = 1 }
                                                , new OrderBy[] { new OrderBy() { FieldName = "CreateTime", Direction = OrderByDirections.Asc } });
                            IEnumerable<CustomerPrizeMappingEntity> prizedInfo;
                            if (drawAll.Length > 0 && arr.Length > 0)
                                prizedInfo = arr.Where(s => s.ID != mapping1 && s.ID != mapping2 && s.ID != mapping3).ToArray();
                            else
                                prizedInfo = arr;

                            if (drawAll.Length > 0)//已经中满一套
                            {
                                int random = new Random().Next(1, _random);
                                if (random == 1)
                                {
                                    if (prizedInfo.Count() == 0)
                                    {
                                        int randomPrize = new Random().Next(1, 3);
                                        var mapping = new CustomerPrizeMappingEntity();
                                        mapping.Openid = request.OpenID;
                                        mapping.PrizeID = randomPrize;
                                        mapping.Enable = 0;
                                        new CustomerPrizeMappingBLL(new BasicUserInfo()).Create(mapping);

                                        response.Data.PreDrawLotteryResult = 1;
                                        response.Data.PrizeID = randomPrize;
                                        response.Data.MappingID = mapping.ID.Value;
                                    }
                                    else if (prizedInfo.Count() == 1)
                                    {
                                        int[] arrRandom = new int[] { 1, 2, 3 };
                                        foreach (int tmp in arrRandom)
                                        {
                                            if (prizedInfo.Where(s => s.PrizeID.Value == tmp).Count() == 0)
                                            {
                                                var mapping = new CustomerPrizeMappingEntity();
                                                mapping.Openid = request.OpenID;
                                                mapping.PrizeID = tmp;
                                                mapping.Enable = 0;
                                                new CustomerPrizeMappingBLL(new BasicUserInfo()).Create(mapping);

                                                response.Data.PreDrawLotteryResult = 1;
                                                response.Data.PrizeID = tmp;
                                                response.Data.MappingID = mapping.ID.Value;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                int random = new Random().Next(1, _random);
                                if (random == 1)
                                {
                                    if (prizedInfo.Count() == 0)
                                    {
                                        int randomPrize = new Random().Next(1, 3);
                                        var mapping = new CustomerPrizeMappingEntity();
                                        mapping.Openid = request.OpenID;
                                        mapping.PrizeID = randomPrize;
                                        mapping.Enable = 0;
                                        new CustomerPrizeMappingBLL(new BasicUserInfo()).Create(mapping);

                                        response.Data.PreDrawLotteryResult = 1;
                                        response.Data.PrizeID = randomPrize;
                                        response.Data.MappingID = mapping.ID.Value;
                                    }
                                    else if (prizedInfo.Count() < 3)
                                    {
                                        int[] arrRandom = new int[] { 1, 2, 3 };
                                        foreach(int tmp in arrRandom)
                                        {
                                            if (prizedInfo.Where(s => s.PrizeID.Value == tmp).Count() == 0)
                                            {
                                                var mapping = new CustomerPrizeMappingEntity();
                                                mapping.Openid = request.OpenID;
                                                mapping.PrizeID = tmp;
                                                mapping.Enable = 0;
                                                new CustomerPrizeMappingBLL(new BasicUserInfo()).Create(mapping);

                                                response.Data.PreDrawLotteryResult = 1;
                                                response.Data.PrizeID = tmp;
                                                response.Data.MappingID = mapping.ID.Value;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Code = "103";
                response.Description = "数据库操作错误";

                Loggers.Exception(new ExceptionLogInfo(ex));
            }

            content = response.ToJSON();
            return content;
        }

        public string CompleteDrawLottery()
        {
            string content = string.Empty;

            var response = new CompleteDrawLotteryReponse();
            try
            {
                string reqContent = HttpContext.Current.Request["ReqContent"];

                if (!string.IsNullOrEmpty(reqContent))
                {
                    var request = reqContent.DeserializeJSONTo<CompleteDrawLotteryRequest>();

                    Loggers.Debug(new DebugLogInfo()
                    {
                        Message = string.Format("CompleteDrawLottery: {0}", reqContent)
                    });

                    response.Data = new CompleteDrawLotteryData();
                    response.Data.DrawLotteryResult = 0;
                    response.Data.LotteryNumber = 0;
                    response.Data.PrizeInfoList = new List<PrizeInfoEntity>();
                    response.Data.isEndDraw = 0;
                    var bll = new QualificationBLL(new BasicUserInfo());
                    var firstLogin = bll.QueryByEntity(new QualificationEntity() { Type = 1, EnableFlag = 1, WxOpenID = request.OpenID }, null);
                    if (firstLogin.Length == 1)//第一次抽奖
                    {
                        var entity = firstLogin.FirstOrDefault();
                        var mapping = new CustomerPrizeMappingBLL(new BasicUserInfo()).GetByID(request.MappingID);
                        if (mapping != null)
                        {
                            mapping.Enable = 1;
                            new CustomerPrizeMappingBLL(new BasicUserInfo()).Update(mapping);

                            entity.EnableFlag = 0;
                            bll.Update(entity);
                            response.Data.DrawLotteryResult = mapping.PrizeID.Value;
                        }
                    }
                    else//第N次抽奖
                    {
                        var dailyLogin = bll.QueryEnableQualificationByCurrentTime(request.OpenID);
                        if (dailyLogin.Tables[0].Rows.Count > 0)
                        {
                            var entity = DataTableToObject.ConvertToObject<QualificationEntity>(dailyLogin.Tables[0].Rows[0]);

                            var mapping = new CustomerPrizeMappingBLL(new BasicUserInfo()).GetByID(request.MappingID);
                            if (mapping != null)
                            {
                                mapping.Enable = 1;
                                new CustomerPrizeMappingBLL(new BasicUserInfo()).Update(mapping);

                                entity.EnableFlag = 0;
                                bll.Update(entity);
                                response.Data.DrawLotteryResult = mapping.PrizeID.Value;

                                var drawAll = new DrawAllBLL(loginInfo).QueryByEntity(new DrawAllEntity { Openid = request.OpenID }, null);
                                if (drawAll.Length == 0)
                                {
                                    var arr = new CustomerPrizeMappingBLL(new BasicUserInfo())
                                            .QueryByEntity(new CustomerPrizeMappingEntity() { Openid = request.OpenID, Enable = 1 }
                                            , new OrderBy[] { new OrderBy() { FieldName = "CreateTime", Direction = OrderByDirections.Asc } });
                                    if (arr.Length > 0)
                                    {
                                        if (arr.Where(s => s.PrizeID == 1).Count() > 0
                                            && arr.Where(s => s.PrizeID == 2).Count() > 0
                                            && arr.Where(s => s.PrizeID == 3).Count() > 0)
                                        {
                                            var drawAllEntity = new DrawAllEntity();
                                            drawAllEntity.Openid = request.OpenID;
                                            drawAllEntity.Mapping1ID = arr.Where(s => s.PrizeID == 1).FirstOrDefault().ID;
                                            drawAllEntity.Mapping2ID = arr.Where(s => s.PrizeID == 2).FirstOrDefault().ID;
                                            drawAllEntity.Mapping3ID = arr.Where(s => s.PrizeID == 3).FirstOrDefault().ID;
                                            drawAllEntity.Flag = 0;
                                            new DrawAllBLL(loginInfo).Create(drawAllEntity);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    var first = bll.QueryByEntity(new QualificationEntity() { Type = 1, EnableFlag = 1, WxOpenID = request.OpenID }, null);
                    if (first.Length > 0)
                        response.Data.LotteryNumber += 1;
                    var daily = bll.QueryEnableQualificationByCurrentTime(request.OpenID);
                    if (daily.Tables[0].Rows.Count > 0)
                        response.Data.LotteryNumber += 1;

                    int mapping1 = 0, mapping2 = 0, mapping3 = 0;
                    var drawAll1 = new DrawAllBLL(loginInfo).QueryByEntity(new DrawAllEntity { Openid = request.OpenID,Flag=1 }, null);
                    if (drawAll1.Length > 0)
                    {
                        mapping1 = drawAll1.FirstOrDefault().Mapping1ID.Value;
                        mapping2 = drawAll1.FirstOrDefault().Mapping2ID.Value;
                        mapping3 = drawAll1.FirstOrDefault().Mapping3ID.Value;
                    }

                    var arrMapping = new CustomerPrizeMappingBLL(new BasicUserInfo())
                                    .QueryByEntity(new CustomerPrizeMappingEntity() { Openid = request.OpenID, Enable = 1 }
                                        , new OrderBy[] { new OrderBy() { FieldName = "CreateTime", Direction = OrderByDirections.Asc } });
                    IEnumerable<CustomerPrizeMappingEntity> prizedInfoList;
                    if (drawAll1.Length > 0 && arrMapping.Length > 0)
                        prizedInfoList = arrMapping.Where(s => s.ID != mapping1 && s.ID != mapping2 && s.ID != mapping3).ToArray();
                    else
                        prizedInfoList = arrMapping;

                    foreach (var tmp in prizedInfoList)
                    {
                        response.Data.PrizeInfoList.Add(new PrizeInfoEntity() { PrizeID = tmp.PrizeID.Value });
                    }

                    if (response.Data.DrawLotteryResult > 0)
                        response.Data.isEndDraw = response.Data.PrizeInfoList.Count() == 3 ? 1 : 0;
                }
            }
            catch (Exception ex)
            {
                response.Code = "103";
                response.Description = "数据库操作错误";

                Loggers.Exception(new ExceptionLogInfo(ex));
            }

            content = response.ToJSON();
            return content;
        }

        public string Share()
        {
            string content = string.Empty;

            var response = new ShareResponse();
            try
            {
                string reqContent = HttpContext.Current.Request["ReqContent"];

                if (!string.IsNullOrEmpty(reqContent))
                {
                    var request = reqContent.DeserializeJSONTo<ShareRequest>();

                    Loggers.Debug(new DebugLogInfo()
                    {
                        Message = string.Format("Share: {0}", reqContent)
                    });

                    response.Data = new ShareData();
                    response.Data.LotteryNumber = 0;
                    response.Data.PrizeInfoList = new List<PrizeInfoEntity>();
                    response.Data.ShareResult = 0;
                    var bll = new QualificationBLL(new BasicUserInfo());

                    var dailyShare = bll.QueryShareQualificationByCurrentTime(request.OpenID);
                    if (dailyShare.Tables[0].Rows.Count == 0)
                    {
                        var entity = new QualificationEntity();
                        entity.WxID = "gh_0846d6c7c2b8";
                        entity.WxOpenID = request.OpenID;
                        entity.Type = 2;
                        entity.EnableFlag = 1;
                        entity.UtilityDate = DateTime.Now;
                        bll.Create(entity);
                        response.Data.ShareResult = 1;
                    }

                    var first = bll.QueryByEntity(new QualificationEntity() { Type = 1, EnableFlag = 1, WxOpenID = request.OpenID }, null);
                    if (first.Length > 0)
                        response.Data.LotteryNumber += 1;
                    var daily = bll.QueryEnableQualificationByCurrentTime(request.OpenID);
                    if (daily.Tables[0].Rows.Count > 0)
                        response.Data.LotteryNumber += 1;

                    int mapping1 = 0, mapping2 = 0, mapping3 = 0;
                    var drawAll = new DrawAllBLL(loginInfo).QueryByEntity(new DrawAllEntity { Openid = request.OpenID }, null);
                    if (drawAll.Length > 0)
                    {
                        mapping1 = drawAll.FirstOrDefault().Mapping1ID.Value;
                        mapping2 = drawAll.FirstOrDefault().Mapping2ID.Value;
                        mapping3 = drawAll.FirstOrDefault().Mapping3ID.Value;
                    }

                    var arr = new CustomerPrizeMappingBLL(new BasicUserInfo())
                                    .QueryByEntity(new CustomerPrizeMappingEntity() { Openid = request.OpenID, Enable = 1 }
                                        , new OrderBy[] { new OrderBy() { FieldName = "CreateTime", Direction = OrderByDirections.Asc } });
                    IEnumerable<CustomerPrizeMappingEntity> prizedInfoList;
                    if (drawAll.Length > 0 && arr.Length > 0)
                        prizedInfoList = arr.Where(s => s.ID != mapping1 && s.ID != mapping2 && s.ID != mapping3).ToArray();
                    else
                        prizedInfoList = arr;

                    foreach (var tmp in prizedInfoList)
                    {
                        response.Data.PrizeInfoList.Add(new PrizeInfoEntity() { PrizeID = tmp.PrizeID.Value });
                    }
                }
            }
            catch (Exception ex)
            {
                response.Code = "103";
                response.Description = "数据库操作错误";

                Loggers.Exception(new ExceptionLogInfo(ex));
            }

            content = response.ToJSON();
            return content;
        }

        public string SubmitInfo()
        {
            string content = string.Empty;

            var response = new SubmitInfoResponse();
            try
            {
                string reqContent = HttpContext.Current.Request["ReqContent"];

                if (!string.IsNullOrEmpty(reqContent))
                {
                    var request = reqContent.DeserializeJSONTo<SubmitInfoRequest>();

                    Loggers.Debug(new DebugLogInfo()
                    {
                        Message = string.Format("SubmitInfo: {0}", reqContent)
                    });

                    response.Data = new SubmitInfoData();
                    response.Data.SubmitInfoResult = 0;

                    var bll = new UserInfoBLL(new BasicUserInfo());
                    var ds = bll.QueryByEntity(new UserInfoEntity() { Openid = request.OpenID }, null);

                    var drawAll = new DrawAllBLL(loginInfo).QueryByEntity(new DrawAllEntity { Openid = request.OpenID, Flag = 0 }, null);

                    if (ds.Length == 0 && drawAll.Length > 0)
                    {
                        var entity = new UserInfoEntity();
                        entity.Openid = request.OpenID;
                        entity.Name = request.Name;
                        entity.Address = request.Address;
                        entity.Phone = request.Phone;
                        bll.Create(entity);
                        var mappingBll=new CustomerPrizeMappingBLL(new BasicUserInfo());
                        var mapping1 = mappingBll.GetByID(drawAll.FirstOrDefault().Mapping1ID);
                        mapping1.Enable = 0;
                        mappingBll.Update(mapping1);

                        var mapping2 = mappingBll.GetByID(drawAll.FirstOrDefault().Mapping2ID);
                        mapping2.Enable = 0;
                        mappingBll.Update(mapping2);

                        var mapping3 = mappingBll.GetByID(drawAll.FirstOrDefault().Mapping3ID);
                        mapping3.Enable = 0;
                        mappingBll.Update(mapping3);

                        var entity1 = drawAll.FirstOrDefault();
                        entity1.Flag = 1;
                        new DrawAllBLL(loginInfo).Update(entity1);

                        response.Data.SubmitInfoResult = 1;

                    }
                }
            }
            catch (Exception ex)
            {
                response.Code = "103";
                response.Description = "数据库操作错误";

                Loggers.Exception(new ExceptionLogInfo(ex));
            }

            content = response.ToJSON();
            return content;
        }

    }
}