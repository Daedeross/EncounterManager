namespace Foundation
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Generic implementation of an Update Request Collection
    /// </summary>
    /// <typeparam name="TUpdateRequest"></typeparam>
    [CollectionDataContract(Name = "UpdateRequestCollectionOf{0}", ItemName = "Item")]
    public class UpdateRequestCollection<TUpdateRequest> : List<UpdateRequestCollectionItem<TUpdateRequest>>
    {
    }
}
