using System;
using DowJones.Utilities.Managers;
using Factiva.Gateway.Messages.Assets.Item.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.Utils.V1_0;
using Factiva.Gateway.V1_0;


namespace DowJones.Utilities.Handlers.Items
{
    public class PAMItemManager
    {

        private Image getImageItem(string itemName, string itemDescription, string itemPath, string contentType)
        {
            Image image = new Image();
            image.Properties.Name = itemName;
            image.Properties.Description = itemDescription;
            image.Properties.ImageMimeType = getImageMimeType(contentType);
            image.Properties.FilePath = itemPath;
            return image;
        }

        public ImageMimeType getImageMimeType(string mimeType)
        {
            ImageMimeType imageMimeType;
            switch(mimeType.ToUpper())
            {
                case "JPEG":
                case "JPG":
                    imageMimeType = ImageMimeType.Jpeg;
                    break;
                case "GIF":
                    imageMimeType = ImageMimeType.Gif;
                    break;
                case "PNG":
                    imageMimeType = ImageMimeType.Png;
                    break;
                default:
                    throw new PAMItemManagerException("Invalid mimeType");
            }
            return imageMimeType;
        }
        public string getImageMimeType(ImageMimeType mimeType)
        {

            switch (mimeType)
            {
                case ImageMimeType.Gif:
                    return "GIF";
                case ImageMimeType.Jpeg:
                    return "JPEG";
                case ImageMimeType.Png:
                    return "PNG";
                default:
                    throw new PAMItemManagerException("Invalid mimeType");
            }

        }


        public Item getItem(string itemName, string itemDescription, string itemPath, string itemType, string contentType)
        {
            Item item;
            switch(itemType.ToLower())
            {
                case "image":
                    item = getImageItem(itemName, itemDescription, itemPath, contentType);
                    break;
                default:
                    throw new PAMItemManagerException("Invalid ItemType");
            }
            return item;
        }


        public long CreateItem(Item item, ControlData m_ControlData)
        {
            long itemId = -1;

            try
            {
                // create item
                CreateItemRequest createItemRequest = new CreateItemRequest();
                createItemRequest.Item = item;

                ServiceResponse serviceResponse = ItemService.CreateItem(ControlDataManager.Clone(m_ControlData),
                                                                         createItemRequest);
                if (serviceResponse.rc == 0)
                {
                    object responseObj;
                    long responseObjRC = serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object,
                                                                     out responseObj);
                    if (responseObjRC == 0)
                    {
                        CreateItemResponse createItemResponse = (CreateItemResponse)responseObj;
                        if (createItemResponse != null)
                            itemId = createItemResponse.Id;
                        if (itemId == -1)
                            throw new PAMItemManagerException("Invalid asset id:  -1");
                    }
                    else
                    {
                        throw new PAMItemManagerException(responseObjRC.ToString());
                    }
                   
                }
                else
                {
                    //throw new  PAMItemManagerException(serviceResponse.rc.ToString());    
                    throw new ItemHandlerException(serviceResponse.rc, "createItemRequest failed"); 
                }
                
                return itemId;

            }
            catch (Exception ex)
            {
                //throw new PAMItemManagerException(ex.Message,ex);
                throw ex;
            }
           
        }

        public void UpdateItemName(long itemId, string newName, ControlData m_ControlData)
        {
            
            try
            {
                // Update item name
                UpdateItemNameRequest updateItemNameRequest = new UpdateItemNameRequest();
                updateItemNameRequest.Id = itemId;
                updateItemNameRequest.Name = newName;


                ServiceResponse serviceResponse = ItemService.UpdateItemName(ControlDataManager.Clone(m_ControlData),
                                                                             updateItemNameRequest);
                if (serviceResponse.rc == 0)
                {
                    object responseObj;
                    long responseObjRC = serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object,
                                                                     out responseObj);
                    if (responseObjRC == 0)
                    {
                        UpdateItemNameResponse updateItemNameResponse = (UpdateItemNameResponse)responseObj;
                        if (updateItemNameResponse == null)
                            throw new PAMItemManagerException("Update item name Failed");
                    }
                    else
                        throw new PAMItemManagerException("Update item name Failed");
                   
                }
                else
                    //throw new PAMItemManagerException(serviceResponse.rc.ToString());
                    throw new ItemHandlerException(serviceResponse.rc, "UpdateItemName failed"); 


            }
            catch (Exception ex)
            {
                //throw new PAMItemManagerException(ex.Message,ex);
                throw ex;
            }
           
        }
        public Item getItemById(long itemId, ControlData m_ControlData)
        {
            Item result = null;
           
            try
            {
                // Get item by Id
                GetItemByIDRequest getItemByIdRequest = new GetItemByIDRequest();
                getItemByIdRequest.Id = itemId;

                ServiceResponse serviceResponse = ItemService.GetItemByID(
                    ControlDataManager.Clone(m_ControlData),
                                                                         getItemByIdRequest);
                if (serviceResponse.rc == 0)
                {
                    object responseObj;
                    long responseObjRC = serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object,
                                                                     out responseObj);
                    if (responseObjRC == 0)
                    {
                        GetItemByIDResponse updateItemNameResponse = (GetItemByIDResponse)responseObj;
                        if (updateItemNameResponse != null)
                        {
                            result = updateItemNameResponse.Item;
                            //if (item.GetType().Name.Equals("Image"))
                            //{
                            //    Image image = (Image) item;
                            //    result = image.Properties.FilePath;
                            //    contentType = "IMAGE";
                            //    mimeType = getImageMimeType(image.Properties.ImageMimeType);
                            //}
                        }
                        else
                            throw new PAMItemManagerException("Get Item By Id Failed");
                    }
                    else
                        throw new PAMItemManagerException("Get Item By Id Failed");
                }
                else
                    //throw new PAMItemManagerException(serviceResponse.rc.ToString());
                    throw new ItemHandlerException(serviceResponse.rc, "getItemById failed"); 

                return result;

            }
            catch (Exception ex)
            {
                //throw new PAMItemManagerException(ex.Message,ex);
                throw ex;

            }
           
        }
        
        public void DeleteItem(long itemId, ControlData m_ControlData)
        {
            
            try
            {
                // Get item by Id
                DeleteItemRequest deleteItemRequest = new DeleteItemRequest();
                deleteItemRequest.Id = itemId;

                ServiceResponse serviceResponse = ItemService.DeleteItem(ControlDataManager.Clone(m_ControlData),
                                                                         deleteItemRequest);
                if (serviceResponse.rc == 0)
                {
                    object responseObj;
                    long responseObjRC = serviceResponse.GetResponse(ServiceResponse.ResponseFormat.Object,
                                                                     out responseObj);
                    if (responseObjRC == 0)
                    {
                        DeleteItemResponse deleteItemResponse = (DeleteItemResponse)responseObj;
                        if (deleteItemResponse == null)
                        {
                            throw new PAMItemManagerException("Delete item failed");
                        }
                    }
                    else
                    {
                        throw new PAMItemManagerException("Delete item failed");
                    }

                }
                else
                    //throw new PAMItemManagerException(serviceResponse.rc.ToString());
                    throw new ItemHandlerException(serviceResponse.rc, "DeleteItem failed");

               

            }
            catch (Exception ex)
            {
                //throw new PAMItemManagerException(ex.Message, ex);
                throw ex;
            }

        }
    }
    


    /// <summary>
    /// Exception for PAMItemManager
    /// </summary>
    public class PAMItemManagerException : ApplicationException
    {
        public PAMItemManagerException()
        {
        }

        public PAMItemManagerException(string message)
            : base(message)
        {

        }
        public PAMItemManagerException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}