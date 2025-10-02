using Core.ModelDto.Category;
using Core.ModelDto.EPSPG;
using Core.ModelDto.PaymentGatewayConfig;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using static Core.BaseEnum;

namespace Persistence.Repository
{
    public class EPSPGRepository : IEPSPGRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public EPSPGRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<string> GetPGAuthentication(EPSPGLoginRequestDto _data, PaymentGatewayConfigResponseDto paymentGatewayConfigResponseDto)
        {
            try
            {
                string baseURL = $"{paymentGatewayConfigResponseDto.BaseUrl}/v1/Auth/GetToken";
                string generatedHash = GenerateHash(_data.UserName, paymentGatewayConfigResponseDto.HashKey);
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), baseURL))
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Add("x-hash", generatedHash);
                        string reqString = JsonConvert.SerializeObject(_data);
                        request.Content = new StringContent(reqString, Encoding.UTF8, "application/json");
                        try
                        {
                            var response = await httpClient.SendAsync(request);
                            if (response.IsSuccessStatusCode == true)
                            {
                                using (HttpContent content = response.Content)
                                {
                                    string Ser_success = await content.ReadAsStringAsync();
                                    var deserialized = JsonConvert.DeserializeObject<EPSPGTokenResponseDto>(Ser_success);
                                    return deserialized.Token;
                                }
                            }
                        }
                        catch
                        {
                            return string.Empty;
                        }
                    }
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }

        }



        public async Task<EPSPGPaymentInitialResponsetDto> EPSPGPaymentInitialize(EPSPGPaymentRequestDto ePSPGCheckPaymentStatusRequestDto, PaymentGatewayConfigResponseDto paymentGatewayConfigResponseDto)
        {
            try
            {
                string baseURL = $"{paymentGatewayConfigResponseDto.BaseUrl}/v1/EPSEngine/InitializeEPS";
                string generatedHash = GenerateHash(paymentGatewayConfigResponseDto.UserName, paymentGatewayConfigResponseDto.HashKey);
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), baseURL))
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Add("x-hash", generatedHash);
                        string reqString = JsonConvert.SerializeObject(ePSPGCheckPaymentStatusRequestDto);
                        request.Content = new StringContent(reqString, Encoding.UTF8, "application/json");
                        try
                        {
                            var response = await httpClient.SendAsync(request);
                            if (response.IsSuccessStatusCode == true)
                            {
                                using (HttpContent content = response.Content)
                                {
                                    string Ser_success = await content.ReadAsStringAsync();
                                    var deserialized = JsonConvert.DeserializeObject<EPSPGPaymentInitialResponsetDto>(Ser_success);
                                    return deserialized;
                                }
                            }
                        }
                        catch
                        {
                            return new EPSPGPaymentInitialResponsetDto();
                        }
                    }
                }

                return new EPSPGPaymentInitialResponsetDto();
            }
            catch
            {
                return new EPSPGPaymentInitialResponsetDto();
            }

        }


        public async Task<EPSPGPaymentResponseDto> CheckPaymentStatus(EPSPGCheckPaymentStatusRequestDto checkPaymentStatusDto, PaymentGatewayConfigResponseDto paymentGatewayConfigResponseDto)
        {
            try
            {
                string baseURL = $"{paymentGatewayConfigResponseDto.BaseUrl}/v1/EPSEngine/CheckMerchantTransactionStatus?merchantTransactionId="+checkPaymentStatusDto.merchantId;
                string generatedHash = GenerateHash(checkPaymentStatusDto.merchantId, paymentGatewayConfigResponseDto.HashKey);

                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), baseURL))
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Add("x-hash", generatedHash);
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", checkPaymentStatusDto.Token);
                        try
                        {
                            var response = await httpClient.SendAsync(request);
                            if (response.IsSuccessStatusCode == true)
                            {
                                using (HttpContent content = response.Content)
                                {
                                    string Ser_success = await content.ReadAsStringAsync();
                                    var deserialized = JsonConvert.DeserializeObject<EPSPGPaymentResponseDto>(Ser_success);
                                    return deserialized;
                                }
                            }
                        }
                        catch
                        {
                            return new EPSPGPaymentResponseDto();
                        }
                    }
                }

                return new EPSPGPaymentResponseDto();
            }
            catch
            {
                return new EPSPGPaymentResponseDto();
            }

        }


        public async Task<EPSPGPaymentResponseDto> CheckMerchantPaymentReferenceStatus(EPSPGCheckPaymentStatusRequestDto checkPaymentStatusDto, PaymentGatewayConfigResponseDto paymentGatewayConfigResponseDto)
        {
            try
            {
                string generatedHash = "";
                string baseURL = "";
                if (string.IsNullOrWhiteSpace(checkPaymentStatusDto.paymentReferance)==false)
                {
                    baseURL = $"{paymentGatewayConfigResponseDto.BaseUrl}/v1/EPSEngine/CheckMerchantTransactionStatus?PaymentReferance="+checkPaymentStatusDto.paymentReferance;
                    generatedHash= GenerateHash(checkPaymentStatusDto.paymentReferance, paymentGatewayConfigResponseDto.HashKey);
                }
                else if (string.IsNullOrWhiteSpace(checkPaymentStatusDto.merchantId)==false)
                {
                    baseURL = $"{paymentGatewayConfigResponseDto.BaseUrl}/v1/EPSEngine/CheckMerchantTransactionStatus?merchantTransactionId="+checkPaymentStatusDto.merchantId;

                    generatedHash= GenerateHash(checkPaymentStatusDto.merchantId, paymentGatewayConfigResponseDto.HashKey);
                }
                else if (string.IsNullOrWhiteSpace(checkPaymentStatusDto.epsTransactionId)==false)
                {
                    baseURL = $"{paymentGatewayConfigResponseDto.BaseUrl}/v1/EPSEngine/CheckMerchantTransactionStatus?EPSTransactionId="+checkPaymentStatusDto.epsTransactionId;
                    generatedHash= GenerateHash(checkPaymentStatusDto.epsTransactionId, paymentGatewayConfigResponseDto.HashKey);
                }
                else
                {

                }

                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), baseURL))
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Add("x-hash", generatedHash);
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", checkPaymentStatusDto.Token);
                        try
                        {
                            var response = await httpClient.SendAsync(request);
                            if (response.IsSuccessStatusCode == true)
                            {
                                using (HttpContent content = response.Content)
                                {
                                    string Ser_success = await content.ReadAsStringAsync();
                                    var deserialized = JsonConvert.DeserializeObject<EPSPGPaymentResponseDto>(Ser_success);
                                    return deserialized;
                                }
                            }
                        }
                        catch
                        {
                            return new EPSPGPaymentResponseDto();
                        }
                    }
                }

                return new EPSPGPaymentResponseDto();
            }
            catch
            {
                return new EPSPGPaymentResponseDto();
            }

        }


        public string GenerateHash(string payload = "Default Payload", string hashkey = "Default Hash Key")
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(hashkey)))
            {
                byte[] data = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                return Convert.ToBase64String(data);
            }
        }


        public string UrlGenerator(EPSFinalResponseDto oEPSFinalResponseDto, string url)
        {
            try
            {
                var param = new Dictionary<string, string>() {
                    { "Status", oEPSFinalResponseDto.Status },
                    { "MerchantTransactionId", oEPSFinalResponseDto.MerchantTransactionId },
                    { "ErrorCode", oEPSFinalResponseDto.ErrorCode.ToString() },
                    { "ErrorMessage", oEPSFinalResponseDto.ErrorMessage }
                };

                var newUrl = new Uri(QueryHelpers.AddQueryString(url, param));
                return url +newUrl.PathAndQuery;
            }
            catch (Exception ex)
            {
                var log = ex.Message + "UrlGenerator issue here";
                return null;
            }
        }

        public EPSFinalResponseDto FinalResponseGenerator(EPSFinalResponseDto model, int status, string errorMessage = null)
        {
            if (status == 1)
            {
                model.Status = EPSTransitionStatusEnum.Success.ToString();
            }
            else if (status == 2)
            {
                model.Status = EPSTransitionStatusEnum.Fail.ToString();
                model.ErrorCode = (int)ErrorCodes.Transaction_Failed;
                model.ErrorMessage = "Transaction Failed. Failed Reason: " + errorMessage ?? "";
            }
            else if (status == 3 || status == 4)
            {
                model.Status = EPSTransitionStatusEnum.Cancel.ToString();
                model.ErrorCode = (int)ErrorCodes.Transaction_Canceled;
                model.ErrorMessage = "Transaction Canceled By User";
            }
            else if (status == 5)
            {
                model.Status = EPSTransitionStatusEnum.TokenGeneration.ToString();
                model.ErrorCode = (int)ErrorCodes.Transaction_Failed;
                model.ErrorMessage = "Transaction Failed. Failed Reason: Token Generation Failed";
            }

            return model;
        }
    }
}
