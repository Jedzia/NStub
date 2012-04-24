// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code. Version 3.4.0.38967
// <NameSpace>NStub.CSharp.ObjectGeneration.Builders</NameSpace><Collection>List</Collection><codeType>CSharp</codeType><EnableDataBinding>False</EnableDataBinding><EnableLazyLoading>True</EnableLazyLoading><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>True</HidePrivateFieldInIDE><EnableSummaryComment>True</EnableSummaryComment><VirtualProp>False</VirtualProp><IncludeSerializeMethod>True</IncludeSerializeMethod><UseBaseClass>True</UseBaseClass><GenBaseClass>False</GenBaseClass><BaseClassName>EmptyBuildParametersBase</BaseClassName><GenerateCloneMethod>False</GenerateCloneMethod><GenerateDataContracts>False</GenerateDataContracts><CodeBaseTag>Net35</CodeBaseTag><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><GenerateXMLAttributes>False</GenerateXMLAttributes><EnableEncoding>False</EnableEncoding><AutomaticProperties>True</AutomaticProperties><GenerateShouldSerialize>False</GenerateShouldSerialize><DisableDebug>True</DisableDebug><PropNameSpecified>Default</PropNameSpecified><Encoder>ASCII</Encoder><CustomUsings></CustomUsings><ExcludeIncludedTypes>False</ExcludeIncludedTypes><EnableInitializeFields>True</EnableInitializeFields>
//  </auto-generated>
// ------------------------------------------------------------------------------
namespace NStub.CSharp.ObjectGeneration.Builders {
    using System;
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System.Collections;
    using System.Xml.Schema;
    using System.ComponentModel;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    public partial class BuildParameters : EmptyBuildParametersBase<BuildParameters> {
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private List<BuildParametersOfPropertyBuilder> itemsField;
        
        public List<BuildParametersOfPropertyBuilder> Items {
            get {
                if ((this.itemsField == null)) {
                    this.itemsField = new List<BuildParametersOfPropertyBuilder>();
                }
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    public partial class BuildParametersOfPropertyBuilder : EmptyBuildParametersBase<BuildParametersOfPropertyBuilder> {
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private string methodSuffixField;
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool useDingsField;
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        private int moepField;

        [Category("Property")]
        public string MethodSuffix { get; set; }

        [Category("Property")]
        public bool UseDings { get; set; }

        [Category("Property")]
        public int Moep { get; set; }

    }
}
