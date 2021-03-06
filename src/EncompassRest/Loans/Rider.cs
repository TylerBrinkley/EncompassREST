using System;

namespace EncompassRest.Loans
{
    /// <summary>
    /// Rider
    /// </summary>
    [Entity(SerializeWholeListWhenDirty = true)]
    public sealed partial class Rider : DirtyExtensibleObject, IIdentifiable
    {
        private DirtyValue<string?>? _id;
        private DirtyValue<bool?>? _received;
        private DirtyValue<bool?>? _required;
        private DirtyValue<DateTime?>? _riderDate;
        private DirtyValue<int?>? _riderIndex;
        private DirtyValue<string?>? _riderName;

        /// <summary>
        /// Rider Id
        /// </summary>
        public string? Id { get => _id; set => SetField(ref _id, value); }

        /// <summary>
        /// Rider Received [RIDERNN03]
        /// </summary>
        public bool? Received { get => _received; set => SetField(ref _received, value); }

        /// <summary>
        /// Rider Required [RIDERNN02]
        /// </summary>
        public bool? Required { get => _required; set => SetField(ref _required, value); }

        /// <summary>
        /// Rider Date [RIDERNN04]
        /// </summary>
        public DateTime? RiderDate { get => _riderDate; set => SetField(ref _riderDate, value); }

        /// <summary>
        /// Rider RiderIndex
        /// </summary>
        public int? RiderIndex { get => _riderIndex; set => SetField(ref _riderIndex, value); }

        /// <summary>
        /// Rider Name [RIDERNN01]
        /// </summary>
        public string? RiderName { get => _riderName; set => SetField(ref _riderName, value); }
    }
}