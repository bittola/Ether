﻿using Ether.Core.Constants;
using Ether.Core.Models;
using Ether.Core.Models.VSTS;
using Ether.Core.Reporters.Classifiers;
using Ether.Tests.Infrastructure;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ether.Tests.Classifiers
{
    [TestFixture]
    public class BaseWorkItemsClassifierTest
    {
        BaseWorkItemsClassifier _classifier;

        [SetUp]
        public void SetUp()
        {
            _classifier = new DummyClassifier();
        }

        [Test]
        public void ShouldThrowExceptionIfRequestIsNull()
        {
            _classifier.Invoking(c => c.Classify(null))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test]
        public void ShouldReturnNoneIfWorkItemIsNull()
        {
            var result = _classifier.Classify(new WorkItemResolutionRequest());

            result.Should().NotBeNull();
            result.IsNone.Should().BeTrue();
        }

        [Test]
        public void ShouldReturnNoneIfTeamIsNull()
        {
            var result = _classifier.Classify(new WorkItemResolutionRequest { WorkItem = new VSTSWorkItem() });

            result.Should().NotBeNull();
            result.IsNone.Should().BeTrue();
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("Foo")]
        [TestCase("fasfasf")]
        [TestCase("231")]
        public void ShouldReturnNoneIfUnexpectedOrEmptyType(string type)
        {
            var workItem = new VSTSWorkItem { Fields = new Dictionary<string, string>() };
            workItem.Fields.Add(VSTSFieldNames.WorkItemType, type);

            var result = _classifier.Classify(new WorkItemResolutionRequest { WorkItem = workItem });

            result.Should().NotBeNull();
            result.IsNone.Should().BeTrue();
        }

        [Test]
        public void ShouldReturnResolutionForSupportedType()
        {
            var workItem = new VSTSWorkItem { Fields = new Dictionary<string, string>() };
            workItem.Fields.Add(VSTSFieldNames.WorkItemType, DummyClassifier.SupportedType);
            workItem.Updates = UpdateBuilder.Create().Resolved()
                .Build();

            var result = _classifier.Classify(new WorkItemResolutionRequest { WorkItem = workItem });

            result.Should().NotBeNull();
            result.IsNone.Should().BeFalse();
            result.Resolution.Should().Be(DummyClassifier.ExpectedResolution);
        }

        [Test]
        public void ShouldReturnNoneIfEmptyHistoryItems()
        {
            var workItem = new VSTSWorkItem { Fields = new Dictionary<string, string>() };
            workItem.Fields.Add(VSTSFieldNames.WorkItemType, DummyClassifier.SupportedType);
            workItem.Updates = Enumerable.Empty<WorkItemUpdate>();

            var result = _classifier.Classify(new WorkItemResolutionRequest { WorkItem = workItem });

            result.Should().NotBeNull();
            result.IsNone.Should().BeTrue();
        }

        [Test]
        public void ShouldReturnNoneIfWorkItemUpdatesAreNull()
        {
            var workItem = new VSTSWorkItem { Fields = new Dictionary<string, string>() };
            workItem.Fields.Add(VSTSFieldNames.WorkItemType, DummyClassifier.SupportedType);
            workItem.Updates = null;

            var result = _classifier.Classify(new WorkItemResolutionRequest { WorkItem = workItem });

            result.Should().NotBeNull();
            result.IsNone.Should().BeTrue();
        }

        [Test]
        public void ShouldReturnErrorResolutionIfException()
        {
            var workItem = new VSTSWorkItem { Fields = new Dictionary<string, string>() };
            workItem.Fields.Add(VSTSFieldNames.WorkItemType, ExceptionWorkItemClassifier.SupportedType);
            workItem.Updates = UpdateBuilder.Create().Resolved()
                .Build();

            var classifier = new ExceptionWorkItemClassifier();
            var result = classifier.Classify(new WorkItemResolutionRequest { WorkItem = workItem });

            result.Should().NotBeNull();
            result.IsError.Should().BeTrue();
            result.Reason.Should().Be(ExceptionWorkItemClassifier.ExpectedReason);
        }

    }
}
