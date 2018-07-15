function GetQueryStringParameter(paramToRetrieve) {
    /// <summary>Function to retrieve a query string value.</summary>
    /// <param name="paramToRetrieve" type="String">The querystring parameter to retrieve.</param>
    /// <returns type="String">The value of the querystring parameter.</returns>
    var params =
            document.URL.split("?")[1].split("&");
    var strParams = "";
    for (var i = 0; i < params.length; i = i + 1) {
        var singleParam = params[i].split("=");
        if (singleParam[0] == paramToRetrieve)
            return singleParam[1];
    }
}

$(document).ready(function () {
    var stockSymbol = GetQueryStringParameter("StockSymbol");
    var targetCurrency = GetQueryStringParameter("Currency");
    if (stockSymbol) {
        var url = "https://query.yahooapis.com/v1/public/yql?q=select%20Symbol%2CName%2CDaysLow%2CDaysHigh%2CYearLow%2CYearHigh%2CLastTradePriceOnly%2CLastTradeDate%2CLastTradeTime%2CChange_PercentChange%2CCurrency%20from%20yahoo.finance.quotes%20where%20symbol%20%3D%20%22" + stockSymbol + "%22%09&format=json&diagnostics=true&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";
        var arrQuote, stockPrice, daysHigh, daysLow, yearHigh, yearLow, symbol, company, lastTradeDate, lastTradeTime, arrStockChangePercent, stockChange, stockChangePercent, stockChangeDirection, stockInfoMarkup, currency;
        $("body").addClass("loading");
        $.getJSON(url, function (data) {
            arrQuote = data.query.results.quote;
            stockPrice = parseFloat(Math.round(arrQuote["LastTradePriceOnly"] * 100) / 100).toFixed(2);
            daysHigh = arrQuote["DaysHigh"];
            daysLow = arrQuote["DaysLow"];
            yearHigh = arrQuote["YearHigh"];
            yearLow = arrQuote["YearLow"];
            symbol = arrQuote["Symbol"];
            company = arrQuote["Name"];
            lastTradeDate = arrQuote["LastTradeDate"];
            lastTradeTime = arrQuote["LastTradeTime"];
            arrStockChangePercent = arrQuote["Change_PercentChange"].split(" - ");
            stockChange = arrStockChangePercent[0];
            stockChangePercent = arrStockChangePercent[1];
            stockChangeDirection = stockChange.charAt(0);
            if (stockChangeDirection === "+") {
                stockChangeDirection = "up";
            }
            else {
                stockChangeDirection = "down";
            }
            currency = arrQuote["Currency"];
            if (targetCurrency && currency.toUpperCase() != targetCurrency.toUpperCase()) {
                var currencyPair = currency + targetCurrency;
                var currencyConvertUrl = "https://query.yahooapis.com/v1/public/yql?q=select%20Rate%20from%20yahoo.finance.xchange%20where%20pair%20%3D%20%22" + currencyPair + "%22&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";
                $("body").addClass("loading");
                $.getJSON(currencyConvertUrl, function (response) {
                    var arrCurrencyRate = response.query.results.rate;
                    var exchangeRate = arrCurrencyRate["Rate"];
                    stockPrice = (stockPrice * exchangeRate).toFixed(2);
                    daysHigh = (daysHigh * exchangeRate).toFixed(2);
                    daysLow = (daysLow * exchangeRate).toFixed(2);
                    yearHigh = (yearHigh * exchangeRate).toFixed(2);
                    yearLow = (yearLow * exchangeRate).toFixed(2);
                    stockChange = (stockChange * exchangeRate).toFixed(2);
                    stockInfoMarkup = buildMarkup(stockPrice, daysHigh, daysLow,
                                          yearHigh, yearLow, symbol,
                                          company, lastTradeDate, lastTradeTime,
                                          stockChange, stockChangePercent, stockChangeDirection, targetCurrency);
                    $('.stockInfo').append(stockInfoMarkup);
                })
                .always(function () {
                    $("body").removeClass("loading");
                });
            }
            else {
                if (!targetCurrency) { targetCurrency = currency; }
                stockInfoMarkup = buildMarkup(stockPrice, daysHigh, daysLow,
                                          yearHigh, yearLow, symbol,
                                          company, lastTradeDate, lastTradeTime,
                                          stockChange, stockChangePercent, stockChangeDirection, targetCurrency);
                $('.stockInfo').append(stockInfoMarkup);
            }
        })
        .always(function () {
            $("body").removeClass("loading");
        });
    } else {
        $("body").append("Please configure this web part and provide a stock symbol to display.");
    }
});

function buildMarkup(stockPriceVal, daysHighVal, daysLowVal, yearHighVal, yearLowVal, symbolVal, companyVal, lastTradeDateVal, lastTradeTimeVal, changeVal, changePercentVal, changeDirectionVal, currency) {
    stockInfoMarkup = "<div class=\"companyTitle\"><a href='//finance.yahoo.com/q?s=" + symbolVal + "' target=\"_blank\">" + companyVal + " (Symbol: " + symbolVal + ")</a></div>";
    stockInfoMarkup += "<div class=\"companySymbolDateTime\">" + lastTradeDateVal + " - " + lastTradeTimeVal + " ET</div>";
    stockInfoMarkup += "<span class=\"companyStockPrice\">" + stockPriceVal + "</span>";
    if (changeDirectionVal === "up") {
        stockInfoMarkup += "<span class=\"companyStockUp\">" + changeVal + " (" + changePercentVal + ")</span>";
    }
    else {
        stockInfoMarkup += "<span class=\"companyStockDown\">" + changeVal + " (" + changePercentVal + ")</span>";
    }
    stockInfoMarkup += "<div></div>";
    stockInfoMarkup += "<div>";
    stockInfoMarkup += "<div class=\"companyStockLabels\">";
    stockInfoMarkup += "<div>Days High</div>";
    stockInfoMarkup += "<div>Days Low</div>";
    stockInfoMarkup += "<div>52 Week High</div>";
    stockInfoMarkup += "<div>52 Week Low</div>";
    stockInfoMarkup += "</div>";
    stockInfoMarkup += "<div class=\"companyStockValues\">";
    stockInfoMarkup += "<div>" + daysHighVal + "</div>";
    stockInfoMarkup += "<div>" + daysLowVal + "</div>";
    stockInfoMarkup += "<div>" + yearHighVal + "</div>";
    stockInfoMarkup += "<div>" + yearLowVal + "</div>";
    stockInfoMarkup += "<div class=\"disclaimer\">*Quotes delayed, except where indicated otherwise. Currency in " + currency + ".</div>";
    stockInfoMarkup += "</div>";
    stockInfoMarkup += "</div>";
    return stockInfoMarkup;
}