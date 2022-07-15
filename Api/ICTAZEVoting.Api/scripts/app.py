import cv2
import json
from face_recognition import face_locations, face_encodings, compare_faces
from PIL import Image, ImageEnhance
from numpy import asarray
from flask import Flask, request


def resize_image(image, base_width_):
    resize_ratio = base_width_ / float(image.shape[1])
    # small_image = cv2.resize(image, (0, 0), fx=resize_ratio, fy=resize_ratio)
    # return small_image
    pil_image = Image.fromarray(image)
    small_image = pil_image.resize((base_width_, int(resize_ratio * image.shape[0])))
    arr_image = asarray(small_image)
    return arr_image

def enhance_brightness(image, factor):
    pil_image = Image.fromarray(image)
    brightness_enhancer = ImageEnhance.Brightness(pil_image)
    im_output = brightness_enhancer.enhance(factor)
    arr_image = asarray(im_output)
    return arr_image

def enhance_contrast(image, factor):
    pil_image = Image.fromarray(image)
    contrast_enhancer = ImageEnhance.Contrast(pil_image)
    im_output = contrast_enhancer.enhance(factor)
    arr_image = asarray(im_output)
    return arr_image




app = Flask(__name__)
@app.get("/greeting")
def hello():
    return "Hello world"

@app.post("/verify")
def verify_face():
    if request.is_json:
        req = request.get_json()      
        image1_path = f'../Files/{req["fileName1"]}'
        image2_path = f'../Files/{req["fileName2"]}'
        base_width = 500
        image1 = cv2.imread(image1_path)
        image1 = resize_image(image1, base_width)
        faces_image1 = face_locations(image1)
        face_encodings_image1 = face_encodings(image1) 

        face1_encoding = face_encodings_image1[0]
        image2 = cv2.imread(image2_path)
        image2 = resize_image(image2, base_width)
        faces_image2 = face_locations(image2)
        face_encodings_image2 = face_encodings(image2)       
       

        face_match_bool = False
        face_tolerance = 0.49
        

        if len(faces_image1) == 0 or len(faces_image2) == 0:
            if len(faces_image1) == 0:
                print("No faces detected in the first image")
            if len(faces_image2) == 0:
                print("No faces detected in the second image")

        else:
            face_encoding2 = face_encodings_image2[0]
            face_match_bool = compare_faces([face1_encoding], face_encoding2, tolerance=face_tolerance)[0]
            print("Found {0} faces in the first image".format(len(faces_image1)))
            print("Found {0} faces in the second image".format(len(faces_image2)))            
        
        result = {"match": bool(face_match_bool)}

        return json.dumps(result,indent=2)
  