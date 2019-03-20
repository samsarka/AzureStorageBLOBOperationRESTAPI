namespace AzureBlobStorageRESTAPIOperation
{
    
    using System;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    internal static class Program
    {
        static string StorageAccountName = "samitstorage";
        static string StorageAccountKey = "yvQCeS8/okhvZ6ihl2WMt2bUbt0KrLxB3/5QCxHQchhnOZkWp13XI44Pm1iiesx71ngt9YKv7BQwYH6Utxb2PQ==";

        private static void Main()
        {
            // List the containers in a storage account.

            ListContainersAsyncREST(StorageAccountName, StorageAccountKey, CancellationToken.None).GetAwaiter().GetResult();

            // Delete the Particular container 4th parameter contaner name

            //DeleteContainerAsyncREST(StorageAccountName, StorageAccountKey, CancellationToken.None,"").GetAwaiter().GetResult();


            //Delete a prticumar Blob 4th   container name ; 5th parameter blobnme
            //DeleteBlobAsyncREST(StorageAccountName, StorageAccountKey, CancellationToken.None,"","").GetAwaiter().GetResult();


            Console.WriteLine("Press any key to continue.");
            Console.ReadLine();
        }

        /// <summary>
        /// This is the method to call the Azure Storage REST API .
        /// </summary>
        

        private static async Task DeleteBlobAsyncREST(string storageAccountName, string storageAccountKey, CancellationToken cancellationToken, string containername , string blobname)
        {

            // Construct the URI. This will look like this:
            //   https://myaccount.blob.core.windows.net/resource


            String uri = string.Format("https://{0}.blob.core.windows.net/"+ containername + "/"+ blobname + "", storageAccountName);



            // Set this to whatever payload you desire. Ours is null because 
            //   we're not passing anything in.
            Byte[] requestPayload = null;

            //Instantiate the request message with a null payload.
            using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri)
            { Content = (requestPayload == null) ? null : new ByteArrayContent(requestPayload) })
            {

                // Add the request headers for x-ms-date and x-ms-version.
                DateTime now = DateTime.UtcNow;
                httpRequestMessage.Headers.Add("x-ms-date", now.ToString("R", CultureInfo.InvariantCulture));
                httpRequestMessage.Headers.Add("x-ms-version", "2018-03-28");
                // If you need any additional headers, add them here before creating
                //   the authorization header. 

                // Add the authorization header.
                httpRequestMessage.Headers.Authorization = AzureBlobStorageRESTAPIOperation.GetAuthorizationHeader(
                   storageAccountName, storageAccountKey, now, httpRequestMessage);

                // Send the request.
                using (HttpResponseMessage httpResponseMessage = await new HttpClient().SendAsync(httpRequestMessage, cancellationToken))
                {
                    // If successful (status code = 200), 
                    //   parse the XML response for the container names.
                    if (httpResponseMessage.StatusCode == HttpStatusCode.OK || httpResponseMessage.StatusCode == HttpStatusCode.Accepted)
                    {
                        String xmlString = await httpResponseMessage.Content.ReadAsStringAsync();
                        Console.WriteLine(xmlString.ToString());
                    }
                }
            }
        }
        private static async Task DeleteContainerAsyncREST(string storageAccountName, string storageAccountKey, CancellationToken cancellationToken, string containername)
        {

            // Construct the URI. This will look like this:
            //   https://myaccount.blob.core.windows.net/resource


            String uri = string.Format("https://{0}.blob.core.windows.net/"+ containername + "?restype=container", storageAccountName);

            

            // Set this to whatever payload you desire. Ours is null because 
            //   we're not passing anything in.
            Byte[] requestPayload = null;

            //Instantiate the request message with a null payload.
            using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri)
            { Content = (requestPayload == null) ? null : new ByteArrayContent(requestPayload) })
            {

                // Add the request headers for x-ms-date and x-ms-version.
                DateTime now = DateTime.UtcNow;
                httpRequestMessage.Headers.Add("x-ms-date", now.ToString("R", CultureInfo.InvariantCulture));
                httpRequestMessage.Headers.Add("x-ms-version", "2018-03-28");
                // If you need any additional headers, add them here before creating
                //   the authorization header. 

                // Add the authorization header.
                httpRequestMessage.Headers.Authorization = AzureBlobStorageRESTAPIOperation.GetAuthorizationHeader(
                   storageAccountName, storageAccountKey, now, httpRequestMessage);

                // Send the request.
                using (HttpResponseMessage httpResponseMessage = await new HttpClient().SendAsync(httpRequestMessage, cancellationToken))
                {
                    // If successful (status code = 200), 
                    //   parse the XML response for the container names.
                    if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        String xmlString = await httpResponseMessage.Content.ReadAsStringAsync();
                        Console.WriteLine(xmlString.ToString());
                    }
                }
            }
        }
        private static async Task ListContainersAsyncREST(string storageAccountName, string storageAccountKey, CancellationToken cancellationToken)
        {

            // Construct the URI. This will look like this:
            //   https://myaccount.blob.core.windows.net/resource
            String uri = string.Format("http://{0}.blob.core.windows.net?comp=list", storageAccountName);

            // Set this to whatever payload you desire. Ours is null because 
            //   we're not passing anything in.
            Byte[] requestPayload = null;

            //Instantiate the request message with a null payload.
            using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri)
            { Content = (requestPayload == null) ? null : new ByteArrayContent(requestPayload) })
            {

                // Add the request headers for x-ms-date and x-ms-version.
                DateTime now = DateTime.UtcNow;
                httpRequestMessage.Headers.Add("x-ms-date", now.ToString("R", CultureInfo.InvariantCulture));
                httpRequestMessage.Headers.Add("x-ms-version", "2017-04-17");
                // If you need any additional headers, add them here before creating
                //   the authorization header. 

                // Add the authorization header.
                httpRequestMessage.Headers.Authorization = AzureBlobStorageRESTAPIOperation.GetAuthorizationHeader(
                   storageAccountName, storageAccountKey, now, httpRequestMessage);

                // Send the request.
                using (HttpResponseMessage httpResponseMessage = await new HttpClient().SendAsync(httpRequestMessage, cancellationToken))
                {
                    // If successful (status code = 200), 
                    //   parse the XML response for the container names.
                    if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        String xmlString = await httpResponseMessage.Content.ReadAsStringAsync();
                        XElement x = XElement.Parse(xmlString);
                        foreach (XElement container in x.Element("Containers").Elements("Container"))
                        {
                            Console.WriteLine("Container name = {0}", container.Element("Name").Value);
                        }
                    }
                }
            }
        }
    }
}
