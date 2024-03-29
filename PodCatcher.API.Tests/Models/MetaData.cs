﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PodCatcher.API.Models;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Tests.Models
{
    class MetaDataTest
    {
        [Test]
        public void Create_WithValidUri_ReturnsMetaDataWithLink()
        {
            // Arrange
            var newGuid = Guid.NewGuid();
            
            // Act
            var httpSomeUri = "http://some.uri/";
            var entryPointUri = new Uri(httpSomeUri);
            var metadata = new PodcastMetaData(entryPointUri, newGuid.ToString());

            // Assert
            Assert.AreEqual(metadata.Link, new Uri(httpSomeUri + newGuid));
        }

        [Test]
        public void Create_WithMissingTrailingSlash_ReturnsMetaDataWithLink()
        {
            // Arrange
            var newGuid = Guid.NewGuid();

            // Act
            var httpSomeUri = "http://some.uri/api/";
            var entryPointUri = new Uri("http://some.uri/api");
            var metadata = new PodcastMetaData(entryPointUri, newGuid.ToString());

            // Assert
            Assert.AreEqual(metadata.Link, new Uri(httpSomeUri + newGuid));
        }

        [Test]
        public void Create_WithValidUri_ReturnsMetaDataWithSubscribeLink()
        {
            // Arrange
            var newGuid = Guid.NewGuid();

            // Act
            var httpSomeUri = "http://some.uri/api/";
            var entryPointUri = new Uri("http://some.uri/api");
            var metadata = new PodcastMetaData(entryPointUri, newGuid.ToString());

            // Assert
            Assert.AreEqual(metadata.SubscribeLink, new Uri(httpSomeUri + newGuid + "/subscribe"));
        }
    }
}
