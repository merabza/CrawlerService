namespace CrawlerService.Application.Repositories;

public interface ICrawlerRepositoryCreatorFactory
{
    ICrawlerRepository GetCrawlerRepository();
}
