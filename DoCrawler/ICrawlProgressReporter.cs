using System.Threading;
using System.Threading.Tasks;

namespace DoCrawler;

//პროცესის მიმდინარეობის ანგარიში მონიტორინგისთვის. იმპლემენტირდება CrawlerReCounter-ში
public interface ICrawlProgressReporter
{
    Task SetLength(int length, CancellationToken cancellationToken = default);
    ValueTask IncreasePosition(CancellationToken cancellationToken = default);
    Task SetMessage(string message, CancellationToken cancellationToken = default);
}
