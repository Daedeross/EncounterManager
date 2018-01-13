namespace Foundation.ServiceFabric
{
    public interface IResponseBuilder
    {
        /// <summary>
        /// Builds a <see cref="Response"/> from this <see cref="IResponseBuilder"/>.
        /// </summary>
        /// <returns><see cref="Response"/></returns>
        Response Build();
    }
}