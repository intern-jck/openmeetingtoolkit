﻿namespace Scrapers;
using System.Reflection;

public record ScrapeTarget(string Name, string Url,
    string RowXPath,
    string NameXpath,
    string LocationXpath,
    string TimeXPath,
    string MoreInfoXPath)
{

}

public record ScrapedMeeting(string Name,
    string Location,
    string Time,
    string MoreInfo);

public static class NorthCarolinaScrapeTarget
{
    public static IEnumerable<ScrapeTarget> All { get; } = typeof(NorthCarolinaScrapeTarget).GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(ScrapeTarget))
                .Select(p => (ScrapeTarget)p.GetValue(null)!);

    public static ScrapeTarget Alamance =>
        new ScrapeTarget("Alamance County, NC",
            "https://www.alamance-nc.com/boardscommittees/",
            RowXPath: "//*[@id=\"pe-maincontent\"]/article/div/div/div/ul/li/a",
            NameXpath: ".//*[@id=\"pe-maincontent\"]/article/header/h1",
            LocationXpath: ".//*[@id=\"pe-maincontent\"]/article/div/p[starts-with(.,'Meeting Location:')]", //GOTCHA starts-with handled by ExtractSingleNode
            TimeXPath: ".//*[@id=\"pe-maincontent\"]/article/div/p[starts-with(.,'Meeting Schedule:')]", //tempral expression
            MoreInfoXPath: "//*[@id=\"pe-maincontent\"]/article/div/p[starts-with(.,'Contact Person and Phone:')]" //or use the url?
            );

    public static ScrapeTarget Cumberland =>
        new ScrapeTarget("Cumberland County, NC",
            "https://www.cumberlandcountync.gov/departments/commissioners-group/commissioners/appointed-boards/board-descriptions",
            ".//li[@data-sf-provider = 'OpenAccessProvider']",
            ".//button",
            ".//div[@data-sf-field = 'Location']", //listed as a tempral expression
            ".//div[@data-sf-field = 'Meetings']",
            ""); //no info, just use the default (page url)

    public static ScrapeTarget NewHannover => //https://swagit.com/rock-solid-technologies-acquires-swagit/
        new ScrapeTarget("New Hannover County, NC",
            "http://commissioners.nhcgov.com/?plugin=all-in-one-event-calendar&controller=ai1ec_exporter_controller&action=export_events&xml=true",
            "//vevent",
            ".//summary/text",
            ".//location/text", //or .//geo/latitude .//geo/longitude
            ".//dtstart/date-time", //listed per each date
            ".//url/uri");

    public static ScrapeTarget Avery => //https://www.revize.com/government-cms.html
        new ScrapeTarget("Avery County, NC",
            "https://cms3.revize.com/revize/plugins/calendar/editpages/export_events.jsp?webspaceId=averycounty&CAL_ID=1&timezoneid=America/New_York",
            "",
            "",
            "",
            "",
            "");
}
