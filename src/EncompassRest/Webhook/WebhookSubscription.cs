﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using EncompassRest.Utilities;
using EnumsNET;
using Newtonsoft.Json;

namespace EncompassRest.Webhook
{
    [Entity(PropertiesToAlwaysSerialize = nameof(Events))]
    public sealed class WebhookSubscription : DirtyExtensibleObject, IIdentifiable
    {
        private string _endpoint;
        private string _subscriptionId;
        private StringEnumValue<WebhookResourceType> _resource;
        private DirtyList<StringEnumValue<WebhookResourceEvent>> _events;
        private WebhookFilters _filters;
        private NeverSerializeValue<string> _objectUrn;
        private NeverSerializeValue<string> _clientId;
        private NeverSerializeValue<string> _instanceId;

        [JsonRequired]
        public string Endpoint { get => _endpoint; set => SetField(ref _endpoint, value); }

        public string SubscriptionId { get => _subscriptionId; set => SetField(ref _subscriptionId, value); }

        [JsonRequired]
        public StringEnumValue<WebhookResourceType> Resource { get => _resource; set => SetField(ref _resource, value); }

        [JsonRequired]
        public IList<StringEnumValue<WebhookResourceEvent>> Events { get => GetField(ref _events); set => SetField(ref _events, value); }

        public WebhookFilters Filters { get => GetField(ref _filters); set => SetField(ref _filters, value); }

        public string ObjectUrn { get => _objectUrn; set => SetField(ref _objectUrn, value); }

        public string ClientId { get => _clientId; set => SetField(ref _clientId, value); }

        public string InstanceId { get => _instanceId; set => SetField(ref _instanceId, value); }

        [IdPropertyName(nameof(SubscriptionId))]
        string IIdentifiable.Id { get => SubscriptionId; set => SubscriptionId = value; }

        [Obsolete("Use another constructor instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public WebhookSubscription(string endpoint)
        {
            Preconditions.NotNullOrEmpty(endpoint, nameof(endpoint));

            Endpoint = endpoint;
        }

        public WebhookSubscription(string endpoint, WebhookResourceType resource, IEnumerable<WebhookResourceEvent> events)
            : this(endpoint, resource.Validate(nameof(resource)).GetValue(), events?.Select(e => e.Validate(nameof(events)).GetValue()))
        {
        }

        [JsonConstructor]
        public WebhookSubscription(string endpoint, string resource, IEnumerable<string> events)
        {
            Preconditions.NotNullOrEmpty(endpoint, nameof(endpoint));
            Preconditions.NotNullOrEmpty(resource, nameof(resource));
            Preconditions.NotNullOrEmpty(events, nameof(events));

            Endpoint = endpoint;
            Resource = resource;
            _events = new DirtyList<StringEnumValue<WebhookResourceEvent>>(events.Select(e => new StringEnumValue<WebhookResourceEvent>(e)));
        }
    }
}