using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;

public class KuveytApi : MonoBehaviour
{
	private async Task<String> getTokenAsync()
	{
		HttpClient client = new HttpClient();

		var parameters = new Dictionary<string, string>
			{
				{"grant_type", "client_credentials"},
				{"scope", "public"},
				{"client_id", "3dead4890b724c3dac70a3f6f6c4fea6"},
				{"client_secret", "Y7H/4aqzQCjlr2KlIJQn1xQdbtoOKxnd0rtKKBPbwsjC+J2dSM1rpg=="}
			};
		var request = new HttpRequestMessage(HttpMethod.Post, "https://idprep.kuveytturk.com.tr/api/connect/token")
		{ Content = new FormUrlEncodedContent(parameters) };

		using (var result = await client.SendAsync(request))
		{
			string content = await result.Content.ReadAsStringAsync();
			return content;
		}
	}


	public string getToken()
	{
		string content = this.getTokenAsync().Result;

		Console.WriteLine(content);

		var pattern = new Regex("\"access_token\":\"([\\w]+)\"");
		Match match = pattern.Match(content);


		return match.Success ? match.Groups[1].Value : null;
	}

	private async Task<String> getCreditCardInformationAsync(string cardNumber)
	{
		HttpClient client = new HttpClient();
		client.DefaultRequestHeaders.Authorization =
			new AuthenticationHeaderValue("Bearer", getToken());


		String payload = new StringBuilder()
			.Append("\"{\"request\": {\"cardnumber\": \"")
			.Append(cardNumber)
			.Append("\"}}\"")
			.ToString();

		HttpRequestMessage requestMessage =
			new HttpRequestMessage(HttpMethod.Post, "https://apitest.kuveytturk.com.tr/prep/v1/cards/carddetail")
			{
				Content = new StringContent(payload, Encoding.UTF8,
					"application/json")
			};

		HttpResponseMessage response = await client.SendAsync(requestMessage);
		return await response.Content.ReadAsStringAsync();
	}


    public CardInformation getCreditCardInformation(string cardNumber)
    {
        // carddetail servisi http403 donuyor...
        var result = getCreditCardInformationAsync(cardNumber).Result;

        // TODO credit card
        if (cardNumber.Equals("4025916319964789"))
        {
            return new CardInformation(100.0, 10.0, 5, 12, cardNumber);
        }

        return new CardInformation(394, 15, 1, 0, cardNumber);
    }

    public List<string> getCreditCardNumbers()
    {
        var cardNumbers = new List<string>();

        cardNumbers.Add("1");
        cardNumbers.Add("2");

        return cardNumbers;
    }
}

public class CardInformation : MonoBehaviour
{
    public double Limit { get; }
    public double PointAmount { get; }
    public int InstallmentCount { get; }
    public int DueDateRemainingDay { get; }
    public string CardNumber { get; }


    public double Score { get; set; } = 0.0;

    public CardInformation(double limit, double pointAmount, int installmentCount, int dueDateRemainingDay, string CardNumber)
    {
        this.Limit = limit;
        this.PointAmount = pointAmount;
        this.InstallmentCount = installmentCount;
        this.DueDateRemainingDay = dueDateRemainingDay;
        this.CardNumber = CardNumber;
        this.CalculateScore();
    }

    private void CalculateScore()
    {
        this.Score = PointAmount * 90 + Limit / 100 + InstallmentCount * 25 + DueDateRemainingDay * 5;
    }

    public override string ToString()
    {
        return String.Concat(Limit, " ", PointAmount, " ", InstallmentCount, " ", DueDateRemainingDay);
    }
}
