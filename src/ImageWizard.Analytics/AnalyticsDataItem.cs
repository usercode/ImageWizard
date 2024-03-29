﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

namespace ImageWizard.Analytics;

public class AnalyticsDataItem
{
    public long CachedDataSendNotModified { get; set; }
    public long CachedDataSendNotModifiedInBytes { get; set; }
    public long CachedDataSend { get; set; }    
    public long CachedDataSendInBytes { get; set; }    
    public long InvalidSignature { get; set; }
    public long ValidSignature { get; set; }
    public long InvalidUrl { get; set; }
    public long CachedDataCreated { get; set; }
    public long CachedDataCreatedInBytes { get; set; }
    public long CachedDataDeleted { get; set; }
}
